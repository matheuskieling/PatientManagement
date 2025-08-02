using FicharioDigital.Model;
using FicharioDigital.Model.DTO;

namespace FicharioDigital.Business.Interfaces;

public interface IUserService
{
    Task RegisterAsync(AuthRequestDto request);
    Task<string> LoginAsync(AuthRequestDto request);
}