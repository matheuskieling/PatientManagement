using FicharioDigital.Model.Enum;

namespace FicharioDigital.Model.DTO;

public record PaymentListingRequestDto(
    Guid? HealthPlanId,
    List<PaymentMethod> PaymentMethods,
    DateTime? StartDate,
    DateTime? EndDate,
    Guid? DoctorId
);