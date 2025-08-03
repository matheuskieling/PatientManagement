using FicharioDigital.Data.Repositories.Interfaces;
using FicharioDigital.Model;
using FicharioDigital.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace FicharioDigital.Data.Repositories;

public class PatientRepository(AppDbContext context) : IPatientRepository
{
    public async Task<PageableResponseDto<Patient>> ListAsync(ListPatientRequestDto request)
    {
        var query = context.Patients.Include(p => p.Contacts).Include(p => p.Category).Include(p => p.HealthPlan).AsQueryable();

        if (request.FileNumber.HasValue)
            query = query.Where(p => EF.Functions.Like(p.FileNumber.ToString(), $"%{request.FileNumber}%"));

        if (request.BirthDate.HasValue)
            query = query.Where(p => p.BirthDate != null && p.BirthDate.Value.Date == request.BirthDate.Value.Date.Date);

        if (!string.IsNullOrEmpty(request.HealthPlan))
            query = query.Where(p => p.HealthPlan != null && EF.Functions.Like(p.HealthPlan.Name, $"%{request.HealthPlan}%"));

        if (!string.IsNullOrEmpty(request.Name))
            query = query.Where(p => EF.Functions.Like(p.Name, $"%{request.Name}%"));

        if (!string.IsNullOrEmpty(request.Cpf))
            query = query.Where(p => EF.Functions.Like(p.Cpf, $"%{request.Cpf}%"));

        if (!string.IsNullOrEmpty(request.Address))
            query = query.Where(p => EF.Functions.Like(p.Address, $"%{request.Address}%"));

        if (!string.IsNullOrEmpty(request.Phones))
            query = query.Where(p => p.Contacts.Any(c => !string.IsNullOrEmpty(c.Phone) && c.Phone.Contains(request.Phones)));
        
        if (!string.IsNullOrEmpty(request.Category))
            query = query.Where(p => p.Category != null && EF.Functions.Like(p.Category.Name, $"%{request.Category}%"));
        
        if (request.IsArchived != null)
            query = query.Where(p => p.IsArchived == request.IsArchived.Value);

        query = query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

        var totalResults = await query.CountAsync();

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

    public async Task<Patient> CreateAsync(Patient patient)
    {
        await context.Patients.AddAsync(patient);
        await context.SaveChangesAsync();
        return patient;
    }
    
    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<long> GetNextPatientNumberAsync()
    {
        var maxFileNumber = await context.Patients.MaxAsync(p => p.FileNumber) ?? 1;
        return maxFileNumber + 1;
    }

    public async Task<Patient?> FindPatientByFileNumberAsync(long fileNumber)
    {
        var patient = await context.Patients.FirstOrDefaultAsync(p => p.FileNumber == fileNumber);
        return patient;
    }
    
    public async Task<Patient?> FindPatientByCpfAsync(string cpf)
    {
        var patient = await context.Patients.FirstOrDefaultAsync(p => p.Cpf == cpf);
        return patient;
    }

    public async Task<Patient?> ValidateAsync(string? name, string? cpf, long? fileNumber)
    {
        var patient = await context.Patients.FirstOrDefaultAsync(p =>
            p.IsArchived == false && (
                (fileNumber.HasValue && p.FileNumber == fileNumber) ||
                (!string.IsNullOrEmpty(name) && p.Name == name) || 
                (!string.IsNullOrEmpty(cpf) && p.Cpf == cpf)
            )
        );
        return patient;
    }

    public async Task<List<Patient>> ListAllAsync()
    {
        return await context.Patients.Where(p => p.FileNumber.HasValue).OrderBy(p => p.FileNumber).ToListAsync();
    }
}