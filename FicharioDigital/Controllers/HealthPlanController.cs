using FicharioDigital.Data.Repositories.Interfaces;
using FicharioDigital.Model;
using FicharioDigital.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FicharioDigital.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class HealthPlanController(IHealthPlanRepository repository) : ControllerBase
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
    
    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid healthPlan id",
                Detail = "HealthPlan id cannot be null.",
                Status = StatusCodes.Status400BadRequest
            });
        }
        
        var healthPlan = await repository.GetHealthPlanById(id);
        if (healthPlan == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "HealthPlan not found",
                Detail = "The healthPlan with the specified id does not exist.",
                Status = StatusCodes.Status404NotFound
            });
        }
        
        await repository.DeleteAsync(healthPlan);
        return NoContent();
    }
}