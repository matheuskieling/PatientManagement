using FicharioDigital.Business.Interfaces;
using FicharioDigital.Data.Repositories.Interfaces;
using FicharioDigital.Model;
using FicharioDigital.Model.DTO;
using FicharioDigital.Model.Mapper;
using Microsoft.EntityFrameworkCore;
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
        
        if (patient.Rg != null)
        {
            var previousPatientWithCpf = await repository.FindPatientByRgAsync(patient.Rg);
            if (previousPatientWithCpf != null)
            {
                throw new InvalidOperationException("O RG informado já está cadastrado. Por favor, verifique os dados e tente novamente.");
            }
        }
        
        if (request.FileNumber != null)
        {
            var previousPatientWithFileNumber = await repository.FindPatientByFileNumberAsync(request.FileNumber.Value);
            if (previousPatientWithFileNumber != null)
            {
                previousPatientWithFileNumber.FileNumber = null;
                await repository.SaveAsync();
            }
        }
        
        if (request.FileNumberEco != null)
        {
            var previousPatientWithFileNumberEco = await repository.FindPatientByFileNumberEcoAsync(request.FileNumberEco.Value);
            if (previousPatientWithFileNumberEco != null)
            {
                previousPatientWithFileNumberEco.FileNumberEco = null;
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
        
        if (!string.IsNullOrEmpty(request.HealthPlanName))
        {
            var healthPlan = await healthPlanRepository.GetHealthPlanByName(request.HealthPlanName);
            if (healthPlan != null)
            {
                patient.HealthPlan = healthPlan;
            }
        }
        return await repository.CreateAsync(patient);
    }
    
    public async Task<Patient> UpdateAsync(PatientRequestDto request)
    {
        var context = repository.GetDbContext();
        using var transaction = await context.Database.BeginTransactionAsync();
    
        try
        {
            var patient = await repository.FindPatientByIdAsync(request.Id!.Value);
            if (patient == null)
            {
                throw new KeyNotFoundException("Paciente não encontrado.");
            }

            // Update patient properties
            patient.Name = request.Name;
            patient.Cpf = request.Cpf;
            patient.Rg = request.Rg;
            patient.BirthDate = request.BirthDate;
            patient.Street = request.Street;
            patient.City = request.City;
            patient.Phone = request.Phone;

            if (request.FileNumber.HasValue)
            {
                var existingPatient = await repository.FindPatientByFileNumberAsync(request.FileNumber.Value);
                if (existingPatient != null)
                {
                    existingPatient.FileNumber = null;
                }
            }
            patient.FileNumber = request.FileNumber;
            
            if (request.FileNumberEco.HasValue)
            {
                var existingPatient = await repository.FindPatientByFileNumberEcoAsync(request.FileNumberEco.Value);
                if (existingPatient != null)
                {
                    existingPatient.FileNumberEco = null;
                }
            }
            patient.FileNumberEco = request.FileNumberEco;

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

            // Remove all existing contacts for this patient
            var existingContacts = await context.Contacts
                .Where(c => c.PatientId == patient.Id)
                .ToListAsync();

            patient.Contacts.Clear();
            foreach (var contact in existingContacts)
            {
                context.Contacts.Remove(contact);
            }

            await context.SaveChangesAsync();
        
            // Add new contacts
            if (request.Contacts.Any())
            {
                var newContacts = request.Contacts.Select(c => new Contact
                {
                    Name = c.Name,
                    Phone = c.Phone,
                    PatientId = patient.Id
                }).ToList();
            
                var contacts = context.Contacts.ToList();
                await context.Contacts.AddRangeAsync(newContacts);
            }

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        
            return patient;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Patient?> GetPatientByCpf(string cpf)
    {
        return await repository.FindPatientByCpfAsync(cpf);
    }
    public async Task<Patient?> GetPatientByRg(string rg)
    {
        return await repository.FindPatientByRgAsync(rg);
    }

    public async Task<NextFileNumbersResponse> GetNextPatientNumberAsync()
    {
        var fileNumbers = await repository.ListAllFileNumbersAsync();
        var fileNumbersEco = await repository.ListAllFileNumbersEcoAsync();
        var index = 1;
        var nextFileNumber = 1;
        var nextFileNumberEco = 1;
        if (!fileNumbers.IsNullOrEmpty())
        {
            foreach (var fileNumber in fileNumbers)
            {
                if (fileNumber != index)
                {
                    nextFileNumber = index;
                    break;
                }
                index++;
            }
            nextFileNumber = index;
        }
        index = 1;
        if (!fileNumbersEco.IsNullOrEmpty())
        {
            foreach (var fileNumber in fileNumbersEco)
            {
                if (fileNumber != index)
                {
                    nextFileNumberEco = index;
                    break;
                }
                index++;
            }

            nextFileNumberEco = index;
        }

        return new NextFileNumbersResponse
        {
            FileNumber = nextFileNumber,
            FileNumberEco = nextFileNumberEco,
        };
    }

    public async Task<ValidationResults> ValidateAsync(PatientRequestDto request)
    {
        var validationResults = new ValidationResults();
        var foundPatients = await repository.ValidateAsync(request.Name, request.Cpf, request.FileNumberEco, request.Rg, request.FileNumber);
        if (foundPatients.IsNullOrEmpty()) return validationResults;
        
        if (request.Name != null && foundPatients.Any(p => p.Name == request.Name))
        {
            var patient = foundPatients.FirstOrDefault(p => p.Name == request.Name);
            var isSamePatient = request.Id != null && patient!.Id == request.Id;
            if (!isSamePatient)
            {
                validationResults.Name.IsValid = false;
                validationResults.Name.ErrorMessage = $"Já existe um paciente cadastrado com o mesmo nome";
            }
        }
        
        if (request.Cpf != null && foundPatients.Any(p => p.Cpf == request.Cpf))
        {
            var patient = foundPatients.FirstOrDefault(p => p.Cpf == request.Cpf);
            var isSamePatient = request.Id != null && patient!.Id == request.Id;
            if (!isSamePatient)
            {
                validationResults.Cpf.IsValid = false;
                validationResults.Cpf.ErrorMessage = $"Não é possível criar o paciente pois o CPF já está cadastrado sob o nome de {patient!.Name}";
            }
        }
        
        if (request.Rg != null && foundPatients.Any(p => p.Rg == request.Rg))
        {
            var patient = foundPatients.FirstOrDefault(p => p.Rg == request.Rg);
            var isSamePatient = request.Id != null && patient!.Id == request.Id;
            if (!isSamePatient)
            {
                validationResults.Rg.IsValid = false;
                validationResults.Rg.ErrorMessage = $"Não é possível criar o paciente pois o RG já está cadastrado sob o nome de {patient!.Rg}";
            }
        }
        
        if (request.FileNumber != null && foundPatients.Any(p => p.FileNumber == request.FileNumber))
        {
            var patient = foundPatients.FirstOrDefault(p => p.FileNumber == request.FileNumber);
            var isSamePatient = request.Id != null && patient!.Id == request.Id;
            if (!isSamePatient)
            {
                validationResults.FileNumber.IsValid = false;
                validationResults.FileNumber.ErrorMessage = $"Já existe um paciente cadastrado com a mesma ficha de consulta. Continuar com a ação irá remover o número da ficha  do paciente {patient!.Name}";
            }
        }
        
        if (request.FileNumberEco != null && foundPatients.Any(p => p.FileNumberEco == request.FileNumberEco))
        {
            var patient = foundPatients.FirstOrDefault(p => p.FileNumberEco == request.FileNumberEco);
            var isSamePatient = request.Id != null && patient!.Id == request.Id;
            if (!isSamePatient)
            {
                validationResults.FileNumberEco.IsValid = false;
                validationResults.FileNumberEco.ErrorMessage = $"Já existe um paciente cadastrado com a mesma ficha de ecografia. Continuar com a ação irá remover o número da ficha de ecografia do paciente {patient!.Name}";
            }
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

    public async Task<long> GetCategoryRemoveCount(Guid categoryId)
    {
        var patients = await repository.GetPatientsByCategoryId(categoryId);
        return patients.Count();
    }

    public async Task<long> RemoveCategoryFromPatientsAsync(Guid categoryId)
    {
        var patients = await repository.GetPatientsByCategoryId(categoryId);
        if (patients.IsNullOrEmpty())
        {
            return 0;
        }

        foreach (var patient in patients)
        {
            patient.Category = null;
        }

        await repository.SaveAsync();
        return patients.Count();
    }

    public async Task<Patient?> GetPatientById(Guid id)
    {
        return await repository.GetPatientById(id);
    }
    
    public async Task<long> GetHealthPlanRemoveCount(Guid healthPlanId)
    {
        var patients = await repository.GetPatientsByHealthPlanId(healthPlanId);
        return patients.Count();
    }

    public async Task<long> RemoveHealthPlanFromPatientsAsync(Guid healthPlanId)
    {
        var patients = await repository.GetPatientsByHealthPlanId(healthPlanId);
        if (patients.IsNullOrEmpty())
        {
            return 0;
        }

        foreach (var patient in patients)
        {
            patient.HealthPlan = null;
        }

        await repository.SaveAsync();
        return patients.Count();
    }
}