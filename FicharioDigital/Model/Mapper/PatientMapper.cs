using FicharioDigital.Model.DTO;

namespace FicharioDigital.Model.Mapper;

public static class PatientMapper
{
    public static Patient ToPatient(this PatientRequestDto request)
    {
        var patient =  new Patient
        {
            FileNumber = request.FileNumber,
            FileNumberEco = request.FileNumberEco,
            BirthDate = request.BirthDate,
            HealthPlanNumber = request.HealthPlanNumber,
            Name = request.Name,
            Cpf = request.Cpf,
            Rg = request.Rg,
            Address = request.Address,
            Gender = request.Gender,
            Phone = request.Phone,
        };
        patient.Contacts = (request.Contacts.Select(c => c.ToContact(patient.Id))).ToList();
        return patient;
    }
}