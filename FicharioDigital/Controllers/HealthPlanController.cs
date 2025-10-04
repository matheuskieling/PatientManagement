using FicharioDigital.Business.Interfaces;
using FicharioDigital.Data.Repositories.Interfaces;
using FicharioDigital.Model;
using FicharioDigital.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FicharioDigital.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class HealthPlanController(IHealthPlanRepository repository, IPatientService patientService, IPaymentService paymentService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var healthPlans = await repository.ListAsync();
        return Ok(healthPlans);
    }
    [HttpPost]
    public async Task<IActionResult> Create(HealthPlanRequestDto request)
    {
        if (string.IsNullOrEmpty(request.Name))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid healthPlan name",
                Detail = "HealthPlan name cannot be empty.",
                Status = StatusCodes.Status400BadRequest
            });
        }
        
        var healthPlan = await repository.CreateHealthPlanAsync(new HealthPlan { Name = request.Name });
        return Ok(healthPlan);
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update(HealthPlanRequestDto request)
    {
        if (request.Id == null || request.Id == Guid.Empty)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid healthPlan id",
                Detail = "HealthPlan id cannot be null.",
                Status = StatusCodes.Status400BadRequest
            });
        }
        
        if (string.IsNullOrEmpty(request.Name))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid healthPlan name",
                Detail = "HealthPlan name cannot be null.",
                Status = StatusCodes.Status400BadRequest
            });
        }
        var healthPlan = await repository.GetHealthPlanById(request.Id.Value);
        if (healthPlan == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "HealthPlan not found",
                Detail = "The healthPlan with the specified id does not exist.",
                Status = StatusCodes.Status404NotFound
            });
        }
        healthPlan.Name = request.Name;
        await repository.SaveAsync();
        return Ok(healthPlan);
    }
    
    [HttpPost("VerifyDelete")]
    public async Task<IActionResult> VerifyDelete(HealthPlanDeleteRequestDto request)
    {
        if (request.Id == Guid.Empty)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid healthPlan id",
                Detail = "HealthPlan id cannot be null.",
                Status = StatusCodes.Status400BadRequest
            });
        }
        var count = await patientService.GetHealthPlanRemoveCount(request.Id);
        return Ok(count);
    }
    
    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(HealthPlanDeleteRequestDto request)
    {
        if (request.Id == Guid.Empty)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid healthPlan id",
                Detail = "HealthPlan id cannot be null.",
                Status = StatusCodes.Status400BadRequest
            });
        }
        
        var healthPlan = await repository.GetHealthPlanById(request.Id);
        if (healthPlan == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "HealthPlan not found",
                Detail = "The healthPlan with the specified id does not exist.",
                Status = StatusCodes.Status404NotFound
            });
        }

        await patientService.RemoveHealthPlanFromPatientsAsync(healthPlan.Id);
        await paymentService.RemovePaymentsHealthPlan(healthPlan.Id);
        
        await repository.DeleteAsync(healthPlan);
        return NoContent();
    }
}