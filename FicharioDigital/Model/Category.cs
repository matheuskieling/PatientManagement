using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FicharioDigital.Model;

[Index(nameof(Name), IsUnique = true)]
public class Category
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Variant { get; set; }
}