using System.Security.Cryptography;
using System.Text;
using FicharioDigital.Business.Interfaces;
using FicharioDigital.Data.Repositories.Interfaces;
using FicharioDigital.Model;
using FicharioDigital.Model.DTO;

namespace FicharioDigital.Business;
public class UserService(IUserRepository repository, ITokenService tokenService) : IUserService
{

    public async Task RegisterAsync(AuthRequestDto request)
    {
        var existingUser = await repository.FindUserByUsername(request.Username);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Username already exists.");
        }
        var salt = GenerateSalt();
        var passwordHash = HashPassword(request.Password, salt);

        var user = new User
        {
            Username = request.Username,
            Salt = salt,
            PasswordHash = passwordHash
        };

        await repository.RegisterAsync(user);
    }

    public async Task<string> LoginAsync(AuthRequestDto request)
    {
        var user = await repository.FindUserByUsername(request.Username);
        if (user == null)
        {
            throw new UnauthorizedAccessException();
        }
        var passwordHash = HashPassword(request.Password, user.Salt);
        if (passwordHash != user.PasswordHash)
        {
            
            throw new UnauthorizedAccessException();
        }

        return tokenService.GenerateToken(user);
    }

    private static string GenerateSalt()
    {
        var saltBytes = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }

    private static string HashPassword(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var saltedPassword = $"{salt}{password}";
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
        return Convert.ToBase64String(hashBytes);
    }
}