using CustomerService.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerService;

public class ApplicationDbContext : DbContext
{

    public DbSet<Customer> Customers { get; set; }
    public DbSet<PaymentInfo> PaymentInfos { get; set; }
    public DbSet<Address> Addresses { get; set; }
    
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public ApplicationDbContext()
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Only configure SQL Server if no options are provided (to avoid overriding options in tests)
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=CustomerServiceDB;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=True;");
        }
    }


}