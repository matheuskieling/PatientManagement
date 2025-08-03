using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace FicharioDigital.Model.DTO;

public class PatientRequestDto
{
     public Guid? Id { get; set; } 
     public long? FileNumber { get; set; } 
     public DateTime? BirthDate { get; set; } 
     public string? HealthPlanName { get; set; } 
     public string? HealthPlanNumber { get; set; } 
     [JsonConverter(typeof(JsonStringEnumConverter<Gender>))]
     public Gender? Gender { get; set; } 
     public string? Name { get; set; } 
     public string? Cpf { get; set; } 
     public string? Phone { get; set; } 
     public string? Address { get; set; }
     public List<ContactRequestDto> Contacts { get; set; } = [];
     public string? CategoryName { get; set; } 
     public bool? IsArchived { get; set; }
};