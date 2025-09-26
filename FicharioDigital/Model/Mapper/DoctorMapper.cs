using FicharioDigital.Model.DTO;

namespace FicharioDigital.Model.Mapper;

public static class DoctorMapper
{
    public static Doctor ToDoctor(DoctorRequestDto dto)
    {
        return new Doctor
        {
            Name = dto.Name
        };
    }
}