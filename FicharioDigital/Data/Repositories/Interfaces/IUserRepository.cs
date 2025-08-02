using FicharioDigital.Model;
using FicharioDigital.Model.DTO;

namespace FicharioDigital.Data.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> RegisterAsync(User user);
    Task<User?> FindUserByUsername(string userName);
}