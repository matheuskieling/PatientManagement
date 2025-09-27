using System.ComponentModel.DataAnnotations;
using FicharioDigital.Model.Enum;

namespace FicharioDigital.Model;

public class Payment
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required] public required string Description { get; set; }
    public HealthPlan? HealthPlan  { get; set; }
    [Required] public required DateTime Date { get; set; } = DateTime.UtcNow;
    [Required] public required PaymentMethod PaymentMethod { get; set; }
    [Required] public required decimal Value { get; set; }
    public Patient? Patient { get; set; }
    [Required] public required bool IsIncome { get; set; }
    public Doctor? Doctor { get; set; }
}