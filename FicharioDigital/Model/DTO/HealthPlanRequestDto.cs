namespace FicharioDigital.Model.DTO;

public record HealthPlanRequestDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
}