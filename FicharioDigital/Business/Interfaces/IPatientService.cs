using FicharioDigital.Model;
using FicharioDigital.Model.DTO;

namespace FicharioDigital.Business.Interfaces;

public interface IPatientService
{
    Task<PageableResponseDto<Patient>> ListAsync(ListPatientRequestDto request);
    Task<Patient> CreateAsync(PatientRequestDto request);
    Task<long> GetNextPatientNumberAsync();
}