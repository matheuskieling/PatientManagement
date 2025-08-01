using FicharioDigital.Model;
using Microsoft.EntityFrameworkCore;

namespace FicharioDigital.Infrastructure;

public static class DependencyInjection
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("Db");
            options.UseSqlite(connectionString ?? throw new ArgumentNullException(nameof(connectionString), "Connection string 'DefaultConnection' not found."));
        });
    }
}