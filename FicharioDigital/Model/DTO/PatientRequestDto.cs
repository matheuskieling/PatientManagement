namespace FicharioDigital.Model.DTO;

public record PatientRequestDto(
     long? FileNumber,
     DateTime? BirthDate,
     string? HealthPlan,
     string? Name,
     string? Cpf,
     string? Address,
     List<ContactRequestDto> Contacts,
     string? Responsible,
     string? CategoryName
);