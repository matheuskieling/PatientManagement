using FicharioDigital.Model;

namespace FicharioDigital.Data.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetCategoryByName(string name);
    Task<Category?> GetCategoryById(Guid id);
    Task<Category> CreateCategoryAsync(Category category);
    Task<List<Category>> ListAsync();
    Task SaveAsync();
    Task DeleteAsync(Category category);
}