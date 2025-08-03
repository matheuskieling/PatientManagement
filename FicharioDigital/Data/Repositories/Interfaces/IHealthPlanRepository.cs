using FicharioDigital.Model;

namespace FicharioDigital.Data.Repositories.Interfaces;

public interface IHealthPlanRepository
{
    Task<HealthPlan?> GetHealthPlanByName(string name);
    Task<HealthPlan?> GetHealthPlanById(Guid id);
    Task<HealthPlan> CreateHealthPlanAsync(HealthPlan category);
    Task<List<HealthPlan>> ListAsync();
    Task SaveAsync();
    Task DeleteAsync(HealthPlan category);
}