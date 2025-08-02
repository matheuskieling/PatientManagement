using FicharioDigital.Business.Interfaces;
using FicharioDigital.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FicharioDigital.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class PatientController(IPatientService service) : ControllerBase
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
    
    [HttpPost("Validate")]
    public async Task<IActionResult> ValidatePatient(PatientRequestDto request)
    {
        var validationResults = await service.ValidateAsync(request);
        return Ok(validationResults);
    }

    [HttpGet("GetNextFileNumber")]
    public async Task<IActionResult> GetNextPatientNumber()
    {
        var nextNumber = await service.GetNextPatientNumberAsync();
        return Ok(new { FileNumber = nextNumber});
    }
    
}