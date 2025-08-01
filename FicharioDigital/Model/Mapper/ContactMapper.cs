using FicharioDigital.Model.DTO;

namespace FicharioDigital.Model.Mapper;

public static class ContactMapper
{
    public static Contact ToContact(this ContactRequestDto request, Guid patientId)
    {
        return new Contact
        {
            Name = request.Name,
            Phone = request.Phone,
            PatientId = patientId
        };
    }
}