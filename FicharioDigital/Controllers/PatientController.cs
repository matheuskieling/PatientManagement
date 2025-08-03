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

    [HttpPost("Update")]
    public async Task<IActionResult> UpdatePatient(PatientRequestDto request)
    {
        try
        {
            var patient = await service.UpdateAsync(request);
            return Ok(patient);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Patient not Found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound,
            });
        }
    }
    
    [HttpPost("Archive/{id}")]
    public async Task<IActionResult> ArchivePatient(Guid id)
    {
        try
        {
            await service.ArchiveAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Patient not Found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound,
            });
        }
    }

    [HttpPost("Delete/{id}")]
    public async Task<IActionResult> DeletePatient(Guid id)
    {
        try
        {
            await service.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Patient not Found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound,
            });
        }
    }
    
    
    [HttpPost("Create")]
    public async Task<IActionResult> CreatePatient(PatientRequestDto request)
    {
        try
        {
            var patient = await service.CreateAsync(request);
            return CreatedAtAction(nameof(CreatePatient), new { patient.Id }, patient);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Patient conflict",
                Detail = ex.Message,
                Status = StatusCodes.Status409Conflict
            });
        }
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