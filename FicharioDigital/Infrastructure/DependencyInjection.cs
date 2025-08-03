using FicharioDigital.Business;
using FicharioDigital.Business.Interfaces;
using FicharioDigital.Data.Repositories;
using FicharioDigital.Data.Repositories.Interfaces;
using FicharioDigital.Model;
using Microsoft.EntityFrameworkCore;

namespace FicharioDigital.Infrastructure;

public static class DependencyInjection
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            options.UseNpgsql(connectionString ?? throw new ArgumentNullException(nameof(connectionString), "Connection string 'DefaultConnection' not found."));
        });
        
        #region Services
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        #endregion
        
        #region Repositories
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        #endregion
    }
}