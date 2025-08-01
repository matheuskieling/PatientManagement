using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FicharioDigital.Model;

[Index(nameof(Nome), IsUnique = true)]
public class Category
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Nome { get; set; }
}