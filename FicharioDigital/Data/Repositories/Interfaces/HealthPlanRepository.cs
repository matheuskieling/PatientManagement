using FicharioDigital.Model;
using Microsoft.EntityFrameworkCore;

namespace FicharioDigital.Data.Repositories.Interfaces;

public class HealthPlanRepository(AppDbContext context) : IHealthPlanRepository
{
    public async Task<HealthPlan?> GetHealthPlanByName(string name)
    {
        var healthPlan = await context.HealthPlans.FirstOrDefaultAsync(c => c.Name == name);
        return healthPlan;
    }
    
    public async Task<HealthPlan?> GetHealthPlanById(Guid id)
    {
        var healthPlan = await context.HealthPlans.FirstOrDefaultAsync(c => c.Id == id);
        return healthPlan;
    }

    public async Task<HealthPlan> CreateHealthPlanAsync(HealthPlan healthPlan)
    {
        await context.HealthPlans.AddAsync(healthPlan);
        await context.SaveChangesAsync();
        return healthPlan;
    }

    public async Task<List<HealthPlan>> ListAsync()
    {
        return await context.HealthPlans.ToListAsync();
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
    public async Task DeleteAsync(HealthPlan healthPlan)
    {
        context.HealthPlans.Remove(healthPlan);
        await context.SaveChangesAsync();
    }
    
}