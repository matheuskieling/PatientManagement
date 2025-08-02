using FicharioDigital.Business.Interfaces;
using FicharioDigital.Model.DTO;
using FicharioDigital.Model.DTO.Reponse;
using Microsoft.AspNetCore.Mvc;

namespace FicharioDigital.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IUserService service) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register(AuthRequestDto request)
    {
        try
        {
            await service.RegisterAsync(request);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Username conflict",
                Detail = ex.Message,
                Status = StatusCodes.Status409Conflict
            });
        }
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login(AuthRequestDto request)
    {
        try
        {
            var token = await service.LoginAsync(request);
            return Ok(new AuthResponseDto
                {
                    Token = token
                }
            );
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Unauthorized",
                Detail = "Invalid username or password.",
                Status = StatusCodes.Status401Unauthorized
            });
        }
    }
}