namespace FicharioDigital.Model.DTO;

public record ListPatientRequestDto(
     long? FileNumber,
     DateTime? BirthDate,
     string? HealthPlan,
     string? Name,
     string? Cpf,
     Gender? Gender,
     string? Address,
     string? Phones,
     string? Category,
     bool? IsArchived,
     int PageNumber = 1,
     int PageSize = 10
);