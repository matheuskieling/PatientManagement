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
    
    public async Task<Category?> GetCategoryById(Guid id)
    {
        var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        return category;
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();
        return category;
    }

    public async Task<List<Category>> ListAsync()
    {
        return await context.Categories.ToListAsync();
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Category category)
    {
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
    }
    
}