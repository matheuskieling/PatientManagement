using System.ComponentModel.DataAnnotations;

namespace FicharioDigital.Model;

public class Doctor
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
}