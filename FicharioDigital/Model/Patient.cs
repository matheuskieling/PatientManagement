using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FicharioDigital.Model;

[Index(nameof(Cpf), IsUnique = true)]
public class Patient
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public long? FileNumber { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? HealthPlan { get; set; }
    public string? Name { get; set; }
    public string? Cpf { get; set; }
    public string? Address { get; set; }
    public string? Phones { get; set; }
    public string? Responsible { get; set; }
    public Category? Category { get; set; }
    public bool IsArchived { get; set; } = false;
}