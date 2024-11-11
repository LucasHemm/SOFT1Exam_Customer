using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CustomerService;
using CustomerService.API; // Ensure this points to your API project's namespace
using CustomerService.DTOs;
using CustomerService.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json; // Or use System.Text.Json
using Testcontainers.MsSql;
using Xunit;

namespace CustomerServiceTest
{
    public class CustomerApiTest : IAsyncLifetime
    {
        private readonly MsSqlContainer _msSqlContainer;
        private readonly WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        public CustomerApiTest()
        {
            _msSqlContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest") // Use the correct SQL Server image
                .WithPassword("YourStrong!Passw0rd") // Set a strong password
                .WithCleanUp(true)
                .Build();

            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Remove the existing ApplicationDbContext registration
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType ==
                                 typeof(DbContextOptions<ApplicationDbContext>));
                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        // Add ApplicationDbContext using the test container's connection string
                        services.AddDbContext<ApplicationDbContext>(options =>
                        {
                            options.UseSqlServer(_msSqlContainer.GetConnectionString());
                        });

                        // Ensure the database is created and migrations are applied
                        var sp = services.BuildServiceProvider();
                        using (var scope = sp.CreateScope())
                        {
                            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                            db.Database.Migrate();
                        }
                    });
                });
        }

        public async Task InitializeAsync()
        {
            await _msSqlContainer.StartAsync();
            _client = _factory.CreateClient();
        }

        public async Task DisposeAsync()
        {
            _client.Dispose();
            await _msSqlContainer.DisposeAsync();
            _factory.Dispose();
        }

        private StringContent GetStringContent(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }


        [Fact]
        public async Task CreateCustomer_ShouldReturnSuccess()
        {
            // Arrange
            var customerDto = new CustomerDTO(
                0,
                "testuser@example.com",
                new PaymentInfoDTO(0, "4111111111111111", "12/25"),
                new AddressDTO(0, "123 Test St", "Testville", "12345")
            );

            // Act
            var response = await _client.PostAsync("/api/CustomerApi", GetStringContent(customerDto));

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("Customer created successfully", responseString);

            // Verify the customer is in the database
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var customer = await context.Customers
                    .Include(c => c.Address)
                    .Include(c => c.PaymentInfo)
                    .FirstOrDefaultAsync(c => c.Email == customerDto.Email);

                Assert.NotNull(customer);
                Assert.Equal(customerDto.Email, customer.Email);
                Assert.Equal(customerDto.PaymentInfoDTO.CardNumber, customer.PaymentInfo.CardNumber);
                Assert.Equal(customerDto.AddressDTO.City, customer.Address.City);
            }
        }

        [Fact]
        public async Task UpdateCustomer_ShouldReturnSuccess()
        {
            // Arrange
            var initialCustomerDto = new CustomerDTO(
                0,
                "updateuser@example.com",
                new PaymentInfoDTO(0, "4222222222222", "11/24"),
                new AddressDTO(0, "456 Initial St", "InitialCity", "54321")
            );

            // Create the initial customer
            var createResponse = await _client.PostAsync("/api/CustomerApi", GetStringContent(initialCustomerDto));
            createResponse.EnsureSuccessStatusCode();

            // Retrieve the created customer's ID
            int customerId;
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var customer = await context.Customers
                    .FirstOrDefaultAsync(c => c.Email == initialCustomerDto.Email);

                Assert.NotNull(customer); // Ensure the customer was created successfully
                customerId = customer.Id;
            }

            // Prepare the updated customer DTO
            var updatedCustomerDto = new CustomerDTO(
                customerId,
                "updateduser@example.com", 
                new PaymentInfoDTO(0, "4333333333333", "10/26"),
                new AddressDTO(0, "789 Updated St", "UpdatedCity", "67890")
            );

            // Act
            var updateResponse = await _client.PutAsync("/api/CustomerApi", GetStringContent(updatedCustomerDto));

            // Assert
            updateResponse.EnsureSuccessStatusCode();
            var updateResponseContent = await updateResponse.Content.ReadAsStringAsync();
            Assert.Equal("Customer updated successfully", updateResponseContent);

            // Verify the customer was updated in the database
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var updatedCustomer = await context.Customers
                    .Include(c => c.Address)
                    .Include(c => c.PaymentInfo)
                    .FirstOrDefaultAsync(c => c.Id == customerId);

                Assert.NotNull(updatedCustomer);
                Assert.Equal(updatedCustomerDto.Email, updatedCustomer.Email);
                Assert.Equal(updatedCustomerDto.PaymentInfoDTO.CardNumber, updatedCustomer.PaymentInfo.CardNumber);
                Assert.Equal(updatedCustomerDto.PaymentInfoDTO.ExpirationDate,
                    updatedCustomer.PaymentInfo.ExpirationDate);
                Assert.Equal(updatedCustomerDto.AddressDTO.Street, updatedCustomer.Address.Street);
                Assert.Equal(updatedCustomerDto.AddressDTO.City, updatedCustomer.Address.City);
                Assert.Equal(updatedCustomerDto.AddressDTO.ZipCode, updatedCustomer.Address.ZipCode);
            }
        }

        [Fact]
        public async Task GetAllCustomers_ShouldReturnAllCreatedCustomers()
        {
            // Arrange
            var customerDtos = new List<CustomerDTO>
            {
                new CustomerDTO(
                    0,
                    "user1@example.com",
                    new PaymentInfoDTO(0, "4444444444444", "09/23"),
                    new AddressDTO(0, "111 First St", "FirstCity", "11111")
                ),
                new CustomerDTO(
                    0,
                    "user2@example.com",
                    new PaymentInfoDTO(0, "5555555555555", "08/22"),
                    new AddressDTO(0, "222 Second St", "SecondCity", "22222")
                )
            };

            foreach (var dto in customerDtos)
            {
                var response = await _client.PostAsync("/api/CustomerApi", GetStringContent(dto));
                response.EnsureSuccessStatusCode();
            }

            // Act
            var getResponse = await _client.GetAsync("/api/CustomerApi");

            // Assert
            getResponse.EnsureSuccessStatusCode();
            var responseString = await getResponse.Content.ReadAsStringAsync();
            var customers = JsonConvert.DeserializeObject<List<CustomerDTO>>(responseString);

            Assert.NotNull(customers);
            Assert.True(customers.Count >= 2); // There might be other customers from other tests

            // Optionally, verify specific customers exist
            Assert.Contains(customers, c => c.Email == "user1@example.com");
            Assert.Contains(customers, c => c.Email == "user2@example.com");
        }
    }
}