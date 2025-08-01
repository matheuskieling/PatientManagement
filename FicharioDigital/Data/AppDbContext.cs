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
            optionsBuilder.UseSqlite("Data Source=FicharioDigital.db; Cache=Shared");
        }
    }
    
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Category> Categories { get; set; }
    
}