using FicharioDigital.Model;

namespace FicharioDigital.Business.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}