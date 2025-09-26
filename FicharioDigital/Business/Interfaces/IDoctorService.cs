using FicharioDigital.Model;
using FicharioDigital.Model.DTO;

namespace FicharioDigital.Business.Interfaces;

public interface IDoctorService
{
    Task<Doctor> CreateAsync(DoctorRequestDto request);
    Task<Doctor> UpdateAsync(DoctorRequestDto requestDto);
    Task<Doctor?> GetDoctorById(Guid id);
    Task<List<Doctor>> ListAsync();
    Task DeleteAsync(Guid id);
}