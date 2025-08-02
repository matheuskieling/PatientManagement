using FicharioDigital.Model;

namespace FicharioDigital.Data.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetCategoryByName(string name);
    Task<Category> CreateCategoryAsync(Category category);
}