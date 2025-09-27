using FicharioDigital.Model.Enum;

namespace FicharioDigital.Model.DTO;

public record PaymentRequestDto(
    Guid? Id,
    string Description,
    Guid? HealthPlanId,
    PaymentMethod PaymentMethod,
    decimal Value,
    Guid? PatientId,
    bool IsIncome,
    Guid? DoctorId
);