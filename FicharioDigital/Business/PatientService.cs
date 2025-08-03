using FicharioDigital.Business.Interfaces;
using FicharioDigital.Data.Repositories.Interfaces;
using FicharioDigital.Model;
using FicharioDigital.Model.DTO;
using FicharioDigital.Model.Mapper;
using Microsoft.IdentityModel.Tokens;

namespace FicharioDigital.Business;

public class PatientService(IPatientRepository repository, ICategoryRepository categoryRepository, IHealthPlanRepository healthPlanRepository) : IPatientService
{
    public async Task<PageableResponseDto<Patient>> ListAsync(ListPatientRequestDto request)
    {
        return await repository.ListAsync(request);
    }

    public async Task<Patient> CreateAsync(PatientRequestDto request)
    {
        var patient = request.ToPatient();
        if (patient.Cpf != null)
        {
            var previousPatientWithCpf = await repository.FindPatientByCpfAsync(patient.Cpf);
            if (previousPatientWithCpf != null)
            {
                throw new InvalidOperationException("O CPF informado já está cadastrado. Por favor, verifique os dados e tente novamente.");
            }
        }
        if (request.FileNumber != null)
        {
            var previousPatientWithFileNumber = await repository.FindPatientByFileNumberAsync(request.FileNumber.Value);
            if (previousPatientWithFileNumber != null)
            {
                previousPatientWithFileNumber.IsArchived = true;
                await repository.SaveAsync();
            }
        }
        if (!string.IsNullOrEmpty(request.CategoryName))
        {
            var category = await categoryRepository.GetCategoryByName(request.CategoryName);
            if (category != null)
            {
                patient.Category = category;
            }
        }
        return await repository.CreateAsync(patient);
    }
    
    public async Task<Patient> UpdateAsync(PatientRequestDto request)
    {
        var patient = await repository.FindPatientByIdAsync(request.Id!.Value);
        if (patient == null)
        {
            throw new KeyNotFoundException("Paciente não encontrado.");
        }
        patient.Name = request.Name;
        patient.Cpf = request.Cpf;
        patient.BirthDate = request.BirthDate;
        patient.Address = request.Address;
        patient.Phone = request.Phone;
        patient.FileNumber = request.FileNumber;
        if (!string.IsNullOrEmpty(request.HealthPlanName))
        {
            var healthPlan = await healthPlanRepository.GetHealthPlanByName(request.HealthPlanName);
            patient.HealthPlan = healthPlan;
        }
        else
        {
            patient.HealthPlan = null;
        }
        
        if (!string.IsNullOrEmpty(request.CategoryName))
        {
            var category = await categoryRepository.GetCategoryByName(request.CategoryName);
            patient.Category = category;
        }
        else
        {
            patient.Category = null;
        }
        patient.HealthPlanNumber = request.HealthPlanNumber;
        patient.Gender = request.Gender;
        patient.Contacts = request.Contacts.Select(c => c.ToContact(patient.Id)).ToList();
        patient.IsArchived = request.IsArchived ?? false;
        await repository.SaveAsync();
        return patient;
    }

    public async Task<long> GetNextPatientNumberAsync()
    {
        var patients = await repository.ListAllAsync();
        var index = 1;
        if (patients.IsNullOrEmpty())
        {
            return 1;
        }
        foreach (var fileNumber in patients.Select(p => p.FileNumber))
        {
            if (fileNumber != index)
            {
                return index;
            }
            index++;
        }

        return index;
    }

    public async Task<ValidationResults> ValidateAsync(PatientRequestDto request)
    {
        var validationResults = new ValidationResults();
        var foundPatient = await repository.ValidateAsync(request.Name, request.Cpf, request.FileNumber);
        if (foundPatient is null) return validationResults;
        
        if (foundPatient.Name == request.Name)
        {
            validationResults.Name.IsValid = false;
            var birthDate = foundPatient.BirthDate?.Date;
            validationResults.Name.ErrorMessage = $"Já existe um paciente cadastrado com o mesmo nome";
            if (birthDate != null)
            {
                validationResults.Name.ErrorMessage += $" e data de nascimento {birthDate.Value:dd/MM/yyyy}";
            }
        }
        
        if (foundPatient.Cpf == request.Cpf)
        {
            validationResults.Cpf.IsValid = false;
            validationResults.Cpf.ErrorMessage = $"Não é possível criar o paciente pois o CPF já está cadastrado sob o nome de {foundPatient.Name}";
        }
        
        if (foundPatient.FileNumber == request.FileNumber)
        {
            validationResults.FileNumber.IsValid = false;
            validationResults.FileNumber.ErrorMessage = $"Já existe um paciente cadastrado com a mesma ficha. Continuar com a ação irá arquivar o paciente {foundPatient.Name}";
        }
        
        return validationResults;
    }
    
    public async Task DeleteAsync(Guid id) 
    {
        var patient = await repository.FindPatientByIdAsync(id);
        if (patient == null)
        {
            throw new KeyNotFoundException("Paciente não encontrado.");
        }
        await repository.DeleteAsync(patient);
    }
    
    public async Task ArchiveAsync(Guid id) 
    {
        var patient = await repository.FindPatientByIdAsync(id);
        if (patient == null)
        {
            throw new KeyNotFoundException("Paciente não encontrado.");
        }
        patient.IsArchived = true;
        await repository.SaveAsync();
    }
    
}