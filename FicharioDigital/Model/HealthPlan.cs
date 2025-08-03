using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FicharioDigital.Model;

[Index(nameof(Name), IsUnique = true)]
public class HealthPlan
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
}