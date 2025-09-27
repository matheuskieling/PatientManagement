using FicharioDigital.Model;
using FicharioDigital.Model.DTO;

namespace FicharioDigital.Data.Repositories.Interfaces;

public interface IPaymentRepository
{
    Task<Payment> CreatePayment(Payment payment);
    Task<Payment?> GetPaymentById(Guid id);
    Task<List<Payment>> ListAsync(PaymentListingRequestDto request);
    Task SaveAsync();
    Task DeleteAsync(Payment payment);
}