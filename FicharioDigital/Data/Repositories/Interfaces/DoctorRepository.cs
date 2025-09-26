using FicharioDigital.Model;
using Microsoft.EntityFrameworkCore;

namespace FicharioDigital.Data.Repositories.Interfaces;

public class DoctorRepository(AppDbContext context) : IDoctorRepository
{
    public async Task<Doctor?> GetDoctorById(Guid id)
    {
        return await context.Doctors.FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Doctor> CreateDoctorAsync(Doctor doctor)
    {
        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();
        return doctor;
    }

    public async Task<List<Doctor>> ListAsync()
    {
        return await context.Doctors.ToListAsync();
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Doctor doctor)
    {
        context.Doctors.Remove(doctor);
        await context.SaveChangesAsync();
    }
}