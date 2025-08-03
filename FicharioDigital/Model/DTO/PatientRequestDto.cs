namespace FicharioDigital.Model.DTO;

public record PatientRequestDto(
     Guid? Id,
     long? FileNumber,
     DateTime? BirthDate,
     string? HealthPlanName,
     string? HealthPlanNumber,
     Gender? Gender,
     string? Name,
     string? Cpf,
     string? Phone,
     string? Address,
     List<ContactRequestDto> Contacts,
     string? CategoryName,
     bool? IsArchived
);