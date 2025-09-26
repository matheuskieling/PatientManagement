using FicharioDigital.Business.Interfaces;
using FicharioDigital.Data.Repositories.Interfaces;
using FicharioDigital.Model;
using FicharioDigital.Model.DTO;
using FicharioDigital.Model.Mapper;

namespace FicharioDigital.Business;

public class DoctorService(IDoctorRepository repository) : IDoctorService
{
    public async Task<Doctor> CreateAsync(DoctorRequestDto request)
    {
        var doctor = DoctorMapper.ToDoctor(request);
        doctor = await repository.CreateDoctorAsync(doctor);
        return doctor;
    }

    public async Task<Doctor> UpdateAsync(DoctorRequestDto requestDto)
    {
        if (!requestDto.Id.HasValue)
            throw new ArgumentException("É necessário informar o ID do médico para atualização.");
        var doctor = await GetDoctorById(requestDto.Id.Value);
        if (doctor is null)
            throw new KeyNotFoundException("Médico não encontrado");
        
        doctor.Name = requestDto.Name;
        await repository.SaveAsync();
        return doctor;
    }

    public async Task<Doctor?> GetDoctorById(Guid id)
    {
        return await repository.GetDoctorById(id);
    }

    public async Task<List<Doctor>> ListAsync()
    {
        return await repository.ListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var doctor = await GetDoctorById(id);
        if (doctor is null)
            throw new KeyNotFoundException("Médico não encontrado");
        await repository.DeleteAsync(doctor);
    }
}