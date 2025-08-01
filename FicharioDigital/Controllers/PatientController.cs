using FicharioDigital.Business;
using FicharioDigital.Business.Interfaces;
using FicharioDigital.Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FicharioDigital.Controllers;

[ApiController]
[Route("[controller]")]
public class PacienteController(IPatientService service) : ControllerBase
{
    [HttpGet("List")]
    public async Task<IActionResult> List([FromQuery] ListPatientRequestDto request)
    {
        var patients = await service.ListAsync(request);
        return Ok(patients);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreatePatient(PatientRequestDto request)
    {
        var patient = await service.CreateAsync(request);
        return CreatedAtAction(nameof(CreatePatient), new { patient.Id }, patient);
    }

    [HttpGet("GetNextFileNumber")]
    public async Task<IActionResult> GetNextPatientNumber()
    {
        var nextNumber = await service.GetNextPatientNumberAsync();
        return Ok(new { FileNumber = nextNumber});
    }
    
}