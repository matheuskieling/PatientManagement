namespace FicharioDigital.Model.DTO;

public record CategoryRequestDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Variant { get; set; }
}