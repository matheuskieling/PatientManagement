using FicharioDigital.Business;
using FicharioDigital.Business.Interfaces;
using FicharioDigital.Data.Repositories.Interfaces;
using FicharioDigital.Model;
using FicharioDigital.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FicharioDigital.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class CategoryController(ICategoryRepository repository, IPatientService patientService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var categories = await repository.ListAsync();
        return Ok(categories);
    }
    [HttpPost]
    public async Task<IActionResult> Create(CategoryRequestDto request)
    {
        if (string.IsNullOrEmpty(request.Name))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid category name",
                Detail = "Category name cannot be empty.",
                Status = StatusCodes.Status400BadRequest
            });
        }
        
        if (string.IsNullOrEmpty(request.Variant))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid category variant",
                Detail = "Category variant cannot be empty.",
                Status = StatusCodes.Status400BadRequest
            });
        }
        var category = await repository.CreateCategoryAsync(new Category { Name = request.Name, Variant = request.Variant});
        return Ok(category);
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update(CategoryRequestDto request)
    {
        if (request.Id == null || request.Id == Guid.Empty)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid category id",
                Detail = "Category id cannot be null.",
                Status = StatusCodes.Status400BadRequest
            });
        }
        
        if (string.IsNullOrEmpty(request.Variant))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid category variant",
                Detail = "Category variant cannot be null.",
                Status = StatusCodes.Status400BadRequest
            });
        }
        
        if (string.IsNullOrEmpty(request.Name))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid category name",
                Detail = "Category name cannot be null.",
                Status = StatusCodes.Status400BadRequest
            });
        }
        var category = await repository.GetCategoryById(request.Id.Value);
        if (category == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Category not found",
                Detail = "The category with the specified id does not exist.",
                Status = StatusCodes.Status404NotFound
            });
        }
        category.Name = request.Name;
        category.Variant = request.Variant;
        await repository.SaveAsync();
        return Ok(category);
    }
    
    [HttpPost("VerifyDelete")]
    public async Task<IActionResult> VerifyDelete(CategoryDeleteRequestDto request)
    {
        if (request.Id == Guid.Empty)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid category id",
                Detail = "Category id cannot be null.",
                Status = StatusCodes.Status400BadRequest
            });
        }
        var count = await patientService.GetCategoryRemoveCount(request.Id);
        return Ok(count);
    }
    
    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(CategoryDeleteRequestDto request)
    {
        if (request.Id == Guid.Empty)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid category id",
                Detail = "Category id cannot be null.",
                Status = StatusCodes.Status400BadRequest
            });
        }
        
        var category = await repository.GetCategoryById(request.Id);
        if (category == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Category not found",
                Detail = "The category with the specified id does not exist.",
                Status = StatusCodes.Status404NotFound
            });
        }

        await patientService.RemoveCategoryFromPatientsAsync(category.Id);
        
        await repository.DeleteAsync(category);
        return NoContent();
    }
}