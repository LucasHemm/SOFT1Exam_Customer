using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using System;
using System.Linq;
using System.Threading.Tasks;
using CustomerService;

namespace SQ_OLA_A2_TEST
{
    public class TestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _msSqlContainer;
        private string _connectionString;

        public TestWebApplicationFactory()
        {
            _msSqlContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest") // Use the correct SQL Server image
                .WithPassword("YourStrong!Passw0rd") // Set a strong password
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Register the DbContext with the Docker-based SQL Server
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(_connectionString);
                });

                // Remove the existing DbContext registration if it exists
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Build the service provider
                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    db.Database.Migrate(); // Apply any pending migrations
                    
                }
            });
        }

        // Implement IAsyncLifetime to manage the container lifecycle
        public async Task InitializeAsync()
        {
            await _msSqlContainer.StartAsync();
            _connectionString = _msSqlContainer.GetConnectionString();
        }

        public new async Task DisposeAsync()
        {
            await _msSqlContainer.DisposeAsync();
            await base.DisposeAsync();
        }
    }
}
