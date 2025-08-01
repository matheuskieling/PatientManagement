using FicharioDigital.Model.DTO;

namespace FicharioDigital.Model.Mapper;

public static class PatientMapper
{
    public static Patient ToPatient(this PatientRequestDto request)
    {
        var patient =  new Patient
        {
            FileNumber = request.FileNumber,
            BirthDate = request.BirthDate,
            HealthPlan = request.HealthPlan,
            Name = request.Name,
            Cpf = request.Cpf,
            Address = request.Address,
            Responsible = request.Responsible,
        };
        patient.Contacts = request.Contacts.Select(c => c.ToContact(patient.Id)).ToList();
        return patient;
    }
}