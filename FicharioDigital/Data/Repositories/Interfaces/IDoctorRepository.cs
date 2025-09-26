using FicharioDigital.Model;

namespace FicharioDigital.Data.Repositories.Interfaces;

public interface IDoctorRepository
{
    Task<Doctor?> GetDoctorById(Guid id);
    Task<Doctor> CreateDoctorAsync(Doctor doctor);
    Task<List<Doctor>> ListAsync();
    Task SaveAsync();
    Task DeleteAsync(Doctor doctor);
}