using System.ComponentModel.DataAnnotations;

namespace FicharioDigital.Model;

public class Contact
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public Guid PatientId { get; set; }
}