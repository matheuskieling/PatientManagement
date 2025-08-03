using FicharioDigital.Model;
using FicharioDigital.Model.DTO;

namespace FicharioDigital.Data.Repositories.Interfaces;

public interface IPatientRepository
{
    Task<PageableResponseDto<Patient>> ListAsync(ListPatientRequestDto request);
    Task<Patient> CreateAsync(Patient patient);
    Task DeleteAsync(Patient patient);
    Task SaveAsync();
    Task<Patient?> ValidateAsync(string? name, string? cpf, long? fileNumber);
    
    Task<Patient?> FindPatientByFileNumberAsync(long fileNumber);
    Task<Patient?> FindPatientByIdAsync(Guid id);
    Task<Patient?> FindPatientByCpfAsync(string cpf);
    Task<List<Patient>> ListAllAsync();
}