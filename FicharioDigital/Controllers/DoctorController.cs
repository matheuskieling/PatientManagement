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
public class DoctorController(IDoctorService service, IPaymentService paymentService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var doctors = await service.ListAsync();
        return Ok(doctors);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(DoctorRequestDto request)
    {
        var doctor = await service.CreateAsync(request);
        return Ok(doctor);
    }

    [HttpPut]
    public async Task<IActionResult> Update(DoctorRequestDto request)
    {
        try
        {
            var payment = await service.UpdateAsync(request);
            return Ok(payment);
        }
        catch (Exception ex) when (ex is KeyNotFoundException || ex is ArgumentException)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Something went wrong",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await paymentService.RemovePaymentsDoctor(id);
            await service.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Something went wrong",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }
}