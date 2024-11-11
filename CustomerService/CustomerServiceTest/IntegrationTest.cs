using CustomerService;
using CustomerService.DTOs;
using CustomerService.Facades;
using CustomerService.Models;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace CustomerServiceTest;

public class IntegrationTest :  IAsyncLifetime
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

            CustomerDTO customerDto = new CustomerDTO(0,"mail", "1234", new PaymentInfoDTO(0,"123456789","26/09"), new AddressDTO(0,"streesttest","citytest","2800"));
            Customer customer = customerFacade.CreateCustomer(customerDto);
            Customer createdCustomer = context.Customers.Find(customer.Id);
            Assert.NotNull(createdCustomer);
            Assert.Equal(customerDto.Email, createdCustomer.Email);
            
        }
    }
    
    

}