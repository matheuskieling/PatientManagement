namespace FicharioDigital.Model.DTO;

public record ListPatientRequestDto(
     long? FileNumber,
     long? FileNumberEco,
     DateTime? BirthDate,
     string? HealthPlan,
     string? Name,
     string? Cpf,
     string? Rg,
     Gender? Gender,
     string? Address,
     string? Phones,
     string? Category,
     bool? IsArchived,
     int PageNumber = 1,
     int PageSize = 10
);