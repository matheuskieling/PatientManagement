using FicharioDigital.Business.Interfaces;
using FicharioDigital.Data.Repositories.Interfaces;
using FicharioDigital.Model;
using FicharioDigital.Model.DTO;
using FicharioDigital.Model.Mapper;
using Microsoft.IdentityModel.Tokens;

namespace FicharioDigital.Business;

public class PaymentService(IPaymentRepository repository,
    IHealthPlanRepository healthPlanRepository,
    IPatientRepository patientRepository,
    IDoctorRepository doctorRepository
    ) : IPaymentService
{
    public async Task<Payment> CreatePayment(PaymentRequestDto paymentRequestDto)
    {
        var payment = PaymentMapper.ToPayment(paymentRequestDto);
        
        if (paymentRequestDto.DoctorId.HasValue)
        {
            var doctor = await doctorRepository.GetDoctorById(paymentRequestDto.DoctorId.Value);
            if (doctor is null)
                throw new KeyNotFoundException("Médico não encontrado");
            payment.Doctor = doctor;
        }
        
        if (paymentRequestDto.HealthPlanId.HasValue)
        {
            var healthPlan = await healthPlanRepository.GetHealthPlanById(paymentRequestDto.HealthPlanId.Value);
            if (healthPlan is null)
                throw new KeyNotFoundException("Convênio não encontrado");
            payment.HealthPlan = healthPlan;
        }
        
        if (paymentRequestDto.PatientId.HasValue)
        {
            var patient = await patientRepository.GetPatientById(paymentRequestDto.PatientId.Value);
            if (patient is null)
                throw new KeyNotFoundException("Paciente não encontrado");
            payment.Patient = patient;
        }
        
        payment = await repository.CreatePayment(payment);
        return payment;
    }

    public async Task<Payment> UpdatePayment(PaymentRequestDto requestDto)
    {
        if (!requestDto.Id.HasValue)
            throw new ArgumentException("É necessário informar o ID do Pagamento para atualização.");
        var payment = await GetPaymentById(requestDto.Id.Value);
        if (payment is null)
            throw new KeyNotFoundException("Médico não encontrado");

        if (!string.IsNullOrEmpty(requestDto.Description))
        {
            payment.Description = requestDto.Description;
        }
        if (requestDto.HealthPlanId.HasValue
            && ((payment.HealthPlan != null && payment.HealthPlan.Id != requestDto.HealthPlanId)
            || payment.HealthPlan == null)
         )
        {
            var healthPlan = await healthPlanRepository.GetHealthPlanById(requestDto.HealthPlanId!.Value);
            if (healthPlan == null)
                throw new KeyNotFoundException("Convênio não encontrado");
            payment.HealthPlan = healthPlan;
        }
        if (requestDto.PatientId.HasValue
            && ((payment.Patient != null && payment.Patient.Id != requestDto.PatientId)
            || payment.Patient == null)
         )
        {
            var patient = await patientRepository.GetPatientById(requestDto.PatientId!.Value);
            if (patient == null)
                throw new KeyNotFoundException("Paciente não encontrado");
            payment.Patient = patient;
        }
        
        if (requestDto.DoctorId.HasValue
            && ((payment.Doctor != null && payment.Doctor.Id != requestDto.DoctorId)
            || payment.Doctor == null)
         )
        {
            var doctor = await doctorRepository.GetDoctorById(requestDto.DoctorId!.Value);
            if (doctor == null)
                throw new KeyNotFoundException("Médico não encontrado");
            payment.Doctor = doctor;
        }
        payment.Value = requestDto.Value;
        payment.IsIncome = requestDto.IsIncome;
        payment.PaymentMethod = requestDto.PaymentMethod;
        
        await repository.SaveAsync();
        return payment;
    }

    public async Task<Payment?> GetPaymentById(Guid id)
    {
        return await repository.GetPaymentById(id);
    }


    public async Task<List<Payment>> ListAsync(PaymentListingRequestDto request)
    {
        return await repository.ListAsync(request);
    }

    public async Task DeleteAsync(Guid id)
    {
        var payment = await GetPaymentById(id);
        if (payment is null)
            throw new KeyNotFoundException("Pagamento não encontrado");
        await repository.DeleteAsync(payment);
        
    }
    
    public async Task<long> RemoveHealthPlanFromPaymentsAsync(Guid healthPlanId)
    {
        var patients = await repository.GetPaymentsByHealthPlan(healthPlanId);
        if (patients.IsNullOrEmpty())
        {
            return 0;
        }

        foreach (var patient in patients)
        {
            patient.HealthPlan = null;
        }

        await repository.SaveAsync();
        return patients.Count();
    }

    public async Task RemovePaymentsHealthPlan(Guid healthPlanId)
    {
        var payments = await repository.GetPaymentsByHealthPlan(healthPlanId);
        foreach (var payment in payments)
        {
            payment.HealthPlan = null;
        }

        await repository.SaveAsync();
    }
    
    public async Task RemovePaymentsDoctor(Guid doctorId)
    {
        var payments = await repository.GetPaymentsByDoctor(doctorId);
        foreach (var payment in payments)
        {
            payment.Doctor = null;
        }

        await repository.SaveAsync();
    }
    
    public async Task RemovePaymentsPatient(Guid patientId)
    {
        var payments = await repository.GetPaymentsByPatient(patientId);
        foreach (var payment in payments)
        {
            payment.Patient = null;
        }

        await repository.SaveAsync();
    }
}