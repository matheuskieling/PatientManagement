using FicharioDigital.Model;
using FicharioDigital.Model.DTO;

namespace FicharioDigital.Business.Interfaces;

public interface IPatientService
{
    Task<PageableResponseDto<Patient>> ListAsync(ListPatientRequestDto request);
    Task<Patient> CreateAsync(PatientRequestDto request);
    Task<Patient?> GetPatientById(Guid id);
    Task<Patient?> GetPatientByCpf(string cpf);
    Task<Patient?> GetPatientByRg(string rg);
    Task<Patient> UpdateAsync(PatientRequestDto request);
    Task DeleteAsync(Guid id);
    Task<ValidationResults> ValidateAsync(PatientRequestDto request);
    Task<NextFileNumbersResponse> GetNextPatientNumberAsync();
    Task<long> RemoveCategoryFromPatientsAsync(Guid categoryId);
    Task<long> GetCategoryRemoveCount(Guid categoryId);
    Task<long> RemoveHealthPlanFromPatientsAsync(Guid healthPlanId);
    Task<long> GetHealthPlanRemoveCount(Guid healthPlanId);
}