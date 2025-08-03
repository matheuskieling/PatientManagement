using FicharioDigital.Model;
using FicharioDigital.Model.DTO;

namespace FicharioDigital.Business.Interfaces;

public interface IPatientService
{
    Task<PageableResponseDto<Patient>> ListAsync(ListPatientRequestDto request);
    Task<Patient> CreateAsync(PatientRequestDto request);
    Task<Patient> UpdateAsync(PatientRequestDto request);
    Task DeleteAsync(Guid id);
    Task ArchiveAsync(Guid id);
    Task<ValidationResults> ValidateAsync(PatientRequestDto request);
    Task<long> GetNextPatientNumberAsync();
}