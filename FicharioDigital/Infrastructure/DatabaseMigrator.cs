using FicharioDigital.Model;
using Microsoft.EntityFrameworkCore;

namespace FicharioDigital.Infrastructure;

public static class DatabaseMigrator
{
    public static void Migrate(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
}