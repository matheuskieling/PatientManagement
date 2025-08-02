using FicharioDigital.Data.Repositories.Interfaces;
using FicharioDigital.Model;
using Microsoft.EntityFrameworkCore;

namespace FicharioDigital.Data.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User> RegisterAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }
    public async Task<User?> FindUserByUsername(string userName)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Username == userName);
    }
}