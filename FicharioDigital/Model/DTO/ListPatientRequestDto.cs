namespace FicharioDigital.Model.DTO;

public record ListPatientRequestDto(
     long? FileNumber,
     DateTime? BirthDate,
     string? HealthPlan,
     string? Name,
     string? Cpf,
     string? Address,
     string? Phones,
     string? Responsible,
     string? Category,
     bool? IsArchived,
     int PageNumber = 1,
     int PageSize = 10
);