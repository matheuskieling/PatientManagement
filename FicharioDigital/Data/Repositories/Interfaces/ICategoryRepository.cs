using FicharioDigital.Model;

namespace FicharioDigital.Data.Repositories.Interfaces;

public interface ICategoryRepository
{
    public Task<Category?> GetCategoryByName(string name);
    public Task<Category> CreateCategoryAsync(Category category);
}