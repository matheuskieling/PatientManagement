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
public class PaymentController(IPaymentService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(PaymentListingRequestDto requestDto)
    {
        var healthPlans = await service.ListAsync(requestDto);
        return Ok(healthPlans);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(PaymentRequestDto request)
    {
        try
        {
            var payment = await service.CreatePayment(request);
            return Ok(payment);
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

    [HttpPut]
    public async Task<IActionResult> Update(PaymentRequestDto request)
    {
        try
        {
            var payment = await service.UpdatePayment(request);
            return Ok(payment);
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
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
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