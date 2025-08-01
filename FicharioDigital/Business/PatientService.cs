using FicharioDigital.Business.Interfaces;
using FicharioDigital.Data.Repositories.Interfaces;
using FicharioDigital.Model;
using FicharioDigital.Model.DTO;
using FicharioDigital.Model.Mapper;

namespace FicharioDigital.Business;

public class PatientService(IPatientRepository repository, ICategoryRepository categoryRepository) : IPatientService
{
    public async Task<PageableResponseDto<Patient>> ListAsync(ListPatientRequestDto request)
    {
        return await repository.ListAsync(request);
    }

    public async Task<Patient> CreateAsync(PatientRequestDto request)
    {
        var patient = request.ToPatient();
        if (!string.IsNullOrEmpty(request.CategoryName))
        {
            var category = await categoryRepository.GetCategoryByName(request.CategoryName);
            patient.Category = category ?? await categoryRepository.CreateCategoryAsync(new Category { Name = request.CategoryName });
        }
        return await repository.CreateAsync(patient);
    }

    public async Task<long> GetNextPatientNumberAsync()
    {
        return await repository.GetNextPatientNumberAsync();
    }
}