using CustomerService;
using CustomerService.DTOs;
using CustomerService.Facades;
using CustomerService.Models;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace CustomerServiceTest;

public class IntegrationTest : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest") // Use the correct SQL Server image
        .WithPassword("YourStrong!Passw0rd") // Set a strong password
        .Build();

    private string _connectionString;

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        // Create the connection string for the database
        _connectionString = _msSqlContainer.GetConnectionString();

        // Initialize the database context and apply migrations
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(_connectionString)
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            context.Database.Migrate(); // Apply any pending migrations
        }
    }

    public async Task DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync().AsTask();
    }

    [Fact]
    public void ShouldCreateCustomer()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(_connectionString)
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            CustomerFacade customerFacade = new CustomerFacade(context);

            CustomerDTO customerDto = new CustomerDTO(0, "mail", new PaymentInfoDTO(0, "123456789", "26/09"),
                new AddressDTO(0, "streesttest", "citytest", "2800"));
            Customer customer = customerFacade.CreateCustomer(customerDto);
            Customer createdCustomer = context.Customers.Find(customer.Id);
            Assert.NotNull(createdCustomer);
            Assert.Equal(customerDto.Email, createdCustomer.Email);
        }
    }

    [Fact]
    public void ShouldUpdateCustomer()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(_connectionString)
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            CustomerFacade customerFacade = new CustomerFacade(context);

            // Create initial customer
            CustomerDTO initialCustomerDto = new CustomerDTO(
                0,
                "initial@mail.com",
                new PaymentInfoDTO(0, "1111222233334444", "12/24"),
                new AddressDTO(0, "Initial Street", "Initial City", "12345")
            );

            Customer initialCustomer = customerFacade.CreateCustomer(initialCustomerDto);

            // Prepare update DTO with provided values
            CustomerDTO updateCustomerDto = new CustomerDTO(
                initialCustomer.Id, // Use the ID of the created customer
                "email2",
                new PaymentInfoDTO(0, "cardnumber2", "expirationdate2"),
                new AddressDTO(0, "street2", "city2", "zipcode2")
            );

            // Act
            Customer updatedCustomer = customerFacade.UpdateCustomer(updateCustomerDto);

            // Assert
            Customer retrievedCustomer = context.Customers
                .Include(c => c.Address)
                .Include(c => c.PaymentInfo)
                .FirstOrDefault(c => c.Id == initialCustomer.Id);

            Assert.NotNull(retrievedCustomer);
            Assert.Equal(updateCustomerDto.Email, retrievedCustomer.Email);
            Assert.Equal(updateCustomerDto.PaymentInfoDTO.CardNumber, retrievedCustomer.PaymentInfo.CardNumber);
            Assert.Equal(updateCustomerDto.PaymentInfoDTO.ExpirationDate, retrievedCustomer.PaymentInfo.ExpirationDate);
            Assert.Equal(updateCustomerDto.AddressDTO.Street, retrievedCustomer.Address.Street);
            Assert.Equal(updateCustomerDto.AddressDTO.City, retrievedCustomer.Address.City);
            Assert.Equal(updateCustomerDto.AddressDTO.ZipCode, retrievedCustomer.Address.ZipCode);
        }
    }


    [Fact]
    public void ShouldReturnAllCustomers()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(_connectionString)
            .Options;
        
        using (var context = new ApplicationDbContext(options))
        {
            CustomerFacade customerFacade = new CustomerFacade(context);
            
            CustomerDTO initialCustomerDto = new CustomerDTO(
                0,
                "initial@mail.com",
                new PaymentInfoDTO(0, "1111222233334444", "12/24"),
                new AddressDTO(0, "Initial Street", "Initial City", "12345")
            );
            
            CustomerDTO initialCustomerDto2 = new CustomerDTO(
                0,
                "initial@mail.com2",
                new PaymentInfoDTO(0, "11112222333344442", "12/242"),
                new AddressDTO(0, "Initial Street2", "Initial City2", "123452")
            );

            Customer initialCustomer = customerFacade.CreateCustomer(initialCustomerDto);

            Customer initialCustomer2 = customerFacade.CreateCustomer(initialCustomerDto2);


            List<Customer> customers = customerFacade.GetAllCustomers();
            
            Assert.Equal(2,customers.Count);
        }
    }
}