using FicharioDigital.Model;
using FicharioDigital.Model.DTO;

namespace FicharioDigital.Business.Interfaces;

public interface IPaymentService
{
    Task<Payment> CreatePayment(PaymentRequestDto paymentRequestDto);
    Task<List<Payment>> ListAsync(PaymentListingRequestDto request);
    Task<Payment> UpdatePayment(PaymentRequestDto request);
    Task DeleteAsync(Guid id);
}