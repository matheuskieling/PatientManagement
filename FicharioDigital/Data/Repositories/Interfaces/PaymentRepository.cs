using FicharioDigital.Model;
using FicharioDigital.Model.DTO;
using FicharioDigital.Model.Enum;
using FicharioDigital.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FicharioDigital.Data.Repositories.Interfaces;

public class PaymentRepository(AppDbContext context) : IPaymentRepository
{
    public async Task<Payment> CreatePayment(Payment payment)
    {
        context.Payments.Add(payment);
        await context.SaveChangesAsync();
        return payment;
    }

    public async Task<List<Payment>> ListAsync(PaymentListingRequestDto request)
    {
        if (!request.StartDate.HasValue)
            request = request with { StartDate = DateTime.UtcNow.Date };
        
        if (!request.EndDate.HasValue)
            request = request with { EndDate = DateTime.UtcNow.Date.AddDays(1) };
        
        if (request.PaymentMethods.IsNullOrEmpty())
            request = request with { PaymentMethods = [ PaymentMethod.Cash ] };
        
        var paymentMethodIds = request.PaymentMethods.Select(pm => (int)pm).ToList();
        
        var query = context.Payments
            .Include(p => p.HealthPlan)
            .Include(p => p.Doctor)
            .Include(p => p.Patient)
            .AsQueryable();

        if (request.DoctorId.HasValue)
            query = query.Where(p => p.Doctor != null && p.Doctor.Id == request.DoctorId);
        
        query = query.Where(p => paymentMethodIds.Contains((int)p.PaymentMethod));
        query = query.Where(p => p.Date >= request.StartDate && p.Date < request.EndDate);
        query = query.OrderBy(p => p.Date);
        
        var sql = query.ToQueryString();
        Console.WriteLine(sql);
        return await query.ToListAsync();
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Payment payment)
    {
        context.Payments.Remove(payment);
        await context.SaveChangesAsync();
    }

    public async Task<Payment?> GetPaymentById(Guid id)
    {
        return await context.Payments
            .Include(p => p.Doctor)
            .Include(p => p.HealthPlan)
            .Include(p => p.Patient)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}