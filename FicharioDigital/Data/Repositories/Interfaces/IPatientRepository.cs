using FicharioDigital.Model;
using FicharioDigital.Model.DTO;

namespace FicharioDigital.Data.Repositories.Interfaces;

public interface IPatientRepository
{
    Task<PageableResponseDto<Patient>> ListAsync(ListPatientRequestDto request);
    Task<Patient> CreateAsync(Patient patient);
    Task DeleteAsync(Patient patient);
    Task SaveAsync();
    Task<List<Patient>> ValidateAsync(string? name, string? cpf, long? fileNumberEco, string? rg, long? fileNumber);
    
    Task<Patient?> FindPatientByFileNumberAsync(long fileNumber);
    Task<Patient?> FindPatientByFileNumberEcoAsync(long fileNumberEco);
    Task<Patient?> FindPatientByIdAsync(Guid id);
    Task<Patient?> FindPatientByCpfAsync(string cpf);
    Task<Patient?> FindPatientByRgAsync(string rg);
    AppDbContext GetDbContext();
    Task<List<long>> ListAllFileNumbersAsync();
    Task<List<long>> ListAllFileNumbersEcoAsync();
    Task<List<Patient>> GetPatientsByCategoryId(Guid categoryId);
    Task<List<Patient>> GetPatientsByHealthPlanId(Guid healthPlanId);
    Task<Patient?> GetPatientById(Guid id);
}