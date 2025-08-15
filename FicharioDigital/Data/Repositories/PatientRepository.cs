using FicharioDigital.Data.Repositories.Interfaces;
using FicharioDigital.Model;
using FicharioDigital.Model.DTO;
using FicharioDigital.Utils;
using Microsoft.EntityFrameworkCore;

namespace FicharioDigital.Data.Repositories;

public class PatientRepository(AppDbContext context) : IPatientRepository
{
    public async Task<PageableResponseDto<Patient>> ListAsync(ListPatientRequestDto request)
    {
        var query = context.Patients.Include(p => p.Contacts).Include(p => p.Category).Include(p => p.HealthPlan)
            .AsQueryable();

        if (request.FileNumber.HasValue)
            query = query.Where(p => p.FileNumber == request.FileNumber.Value);
        
        if (request.FileNumberEco.HasValue)
            query = query.Where(p => p.FileNumberEco == request.FileNumberEco.Value);

        if (request.BirthDate.HasValue)
            query = query.Where(p =>
                p.BirthDate != null && p.BirthDate.Value.Date == request.BirthDate.Value.Date.Date);

        if (!string.IsNullOrEmpty(request.HealthPlan))
            query = query.Where(p =>
                p.HealthPlan != null && EF.Functions.Like(p.HealthPlan.Name, $"%{request.HealthPlan}%"));

        if (!string.IsNullOrEmpty(request.Name))
        {
            var searchTerm = StringUtils.NormalizeString(request.Name);
            var pattern = $"%{searchTerm}%";
            query = query.Where(p => p.Name != null && EF.Functions.ILike(EF.Functions.Unaccent(p.Name), pattern));
        }

        if (!string.IsNullOrEmpty(request.Cpf))
            query = query.Where(p => EF.Functions.Like(p.Cpf, $"%{request.Cpf}%"));
        
        if (!string.IsNullOrEmpty(request.Rg))
            query = query.Where(p => EF.Functions.Like(p.Rg, $"%{request.Rg}%"));

        if (request.Gender.HasValue)
            query = query.Where(p => p.Gender != null && p.Gender == request.Gender);

        if (!string.IsNullOrEmpty(request.Phones))
            query = query.Where(p => p.Phone != null
                                     && (EF.Functions.Like(p.Phone, $"%{request.Phones}%")
                                         || p.Contacts.Any(c =>
                                             !string.IsNullOrEmpty(c.Phone) &&
                                             (EF.Functions.Like(c.Phone, $"%{request.Phones}%")))));

        if (!string.IsNullOrEmpty(request.Category))
            query = query.Where(p => p.Category != null && EF.Functions.Like(p.Category.Name, $"%{request.Category}%"));

        var totalResults = await query.CountAsync();

        query = query.OrderBy(p => p.FileNumber);

        query = query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

        var items = await query.ToListAsync();

        var totalPages = (int)Math.Ceiling(totalResults / (double)request.PageSize);

        return new PageableResponseDto<Patient>
        {
            Page = request.PageNumber,
            PageSize = request.PageSize,
            TotalResults = totalResults,
            TotalPages = totalPages,
            Items = items
        };
    }

    public void RemoveContact(Contact contact)
    {
        context.Contacts.Remove(contact);
    }
    public void ClearChangeTracker()
    {
        context.ChangeTracker.Clear();
    }

    public async Task<Patient> CreateAsync(Patient patient)
    {
        await context.Patients.AddAsync(patient);
        await context.SaveChangesAsync();
        return patient;
    }

    public async Task DeleteAsync(Patient patient)
    {
        context.Patients.Remove(patient);
        await context.SaveChangesAsync();
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<Patient?> FindPatientByFileNumberAsync(long fileNumber)
    {
        var patient = await context.Patients.FirstOrDefaultAsync(p => p.FileNumber == fileNumber);
        return patient;
    }
    
    public async Task<Patient?> FindPatientByFileNumberEcoAsync(long fileNumberEco)
    {
        var patient = await context.Patients.FirstOrDefaultAsync(p => p.FileNumberEco == fileNumberEco);
        return patient;
    }

    public async Task<Patient?> FindPatientByIdAsync(Guid id)
    {
        var patient = await context.Patients
            .Include(p => p.Contacts)
            .Include(p => p.Category)
            .Include(p => p.HealthPlan)
            .FirstOrDefaultAsync(p => p.Id == id);
        return patient;
    }
    
    public AppDbContext GetDbContext()
    {
        return context;
    }

    public async Task<Patient?> FindPatientByCpfAsync(string cpf)
    {
        var patient = await context.Patients.FirstOrDefaultAsync(p => p.Cpf == cpf);
        return patient;
    }
    
    public async Task<Patient?> FindPatientByRgAsync(string rg)
    {
        var patient = await context.Patients.FirstOrDefaultAsync(p => p.Rg == rg);
        return patient;
    }

    public async Task<List<Patient>> ValidateAsync(string? name, string? cpf, long? fileNumberEco, string? rg,
        long? fileNumber)
    {
        var patient = await context.Patients.Where(p =>
            (fileNumber.HasValue && p.FileNumber == fileNumber) ||
            (fileNumberEco.HasValue && p.FileNumberEco == fileNumberEco) ||
            (!string.IsNullOrEmpty(name) && p.Name == name) ||
            (!string.IsNullOrEmpty(cpf) && p.Cpf == cpf) ||
            (!string.IsNullOrEmpty(rg) && p.Rg == rg)
        ).ToListAsync();
        return patient;
    }

    public async Task<List<long>> ListAllFileNumbersAsync()
    {
        return await context.Patients
            .Where(p => p.FileNumber.HasValue)
            .OrderBy(p => p.FileNumber)
            .Select(p => p.FileNumber!.Value)
            .ToListAsync();
    }
    
    public async Task<List<long>> ListAllFileNumbersEcoAsync()
    {
        return await context.Patients
            .Where(p => p.FileNumberEco.HasValue)
            .OrderBy(p => p.FileNumberEco)
            .Select(p => p.FileNumberEco!.Value)
            .ToListAsync();
    }

    public async Task<List<Patient>> GetPatientsByCategoryId(Guid categoryId)
    {
        return await context.Patients.Include(p => p.Category).Where(p => p.Category != null && p.Category.Id == categoryId).ToListAsync();
    }
    
    public async Task<List<Patient>> GetPatientsByHealthPlanId(Guid healthPlanId)
    {
        return await context.Patients.Include(p => p.HealthPlan).Where(p => p.HealthPlan != null && p.HealthPlan.Id == healthPlanId).ToListAsync();
    }

    public async Task<Patient?> GetPatientById(Guid id)
    {
        return await context.Patients
            .Include(p => p.HealthPlan)
            .Include(p => p.Contacts)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}