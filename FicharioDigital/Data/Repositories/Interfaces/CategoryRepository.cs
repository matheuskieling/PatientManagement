using FicharioDigital.Model;
using Microsoft.EntityFrameworkCore;

namespace FicharioDigital.Data.Repositories.Interfaces;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    public async Task<Category?> GetCategoryByName(string name)
    {
        var category = await context.Categories.FirstOrDefaultAsync(c => c.Name == name);
        return category;
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();
        return category;
    }
}