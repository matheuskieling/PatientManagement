using FicharioDigital.Model.DTO;

namespace FicharioDigital.Model.Mapper;

public static class PaymentMapper
{
    public static Payment ToPayment(PaymentRequestDto dto)
    {
        return new Payment
        {
            Description = dto.Description,
            Date = DateTime.UtcNow,
            PaymentMethod = dto.PaymentMethod,
            Value = dto.Value,
            IsIncome = dto.IsIncome,
        };
    }
}