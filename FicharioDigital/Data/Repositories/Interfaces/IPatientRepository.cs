using FicharioDigital.Model;
using FicharioDigital.Model.DTO;

namespace FicharioDigital.Data.Repositories.Interfaces;

public interface IPatientRepository
{
    Task<PageableResponseDto<Patient>> ListAsync(ListPatientRequestDto request);
    Task<Patient> CreateAsync(Patient patient);
    Task SaveAsync();
    Task<long> GetNextPatientNumberAsync();

    Task<Patient?> ValidateAsync(string? name, string? cpf, long? fileNumber);
    
    Task<Patient?> FindPatientByFileNumberAsync(long fileNumber);
}