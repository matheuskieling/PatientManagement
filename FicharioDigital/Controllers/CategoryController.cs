using FicharioDigital.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FicharioDigital.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class CategoryController(ICategoryRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var categories = await repository.ListAsync();
        return Ok(categories);
    }
}