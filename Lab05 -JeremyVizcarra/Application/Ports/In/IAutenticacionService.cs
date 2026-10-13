using Lab05_JeremyVizcarra.Application.Models;

namespace Lab05_JeremyVizcarra.Application.Ports.In;

public interface IAutenticacionService
{
    Task<TokenModel> Login(LoginModel credenciales);
}
