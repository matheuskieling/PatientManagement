using Microsoft.EntityFrameworkCore;

namespace FicharioDigital.Model;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            optionsBuilder.UseNpgsql(connectionString
                                     ?? throw new ArgumentNullException(nameof(connectionString),
                                         "Connection string not found as variable")
            );
        }
    }
    
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<HealthPlan> HealthPlans { get; set; }
    
    
}