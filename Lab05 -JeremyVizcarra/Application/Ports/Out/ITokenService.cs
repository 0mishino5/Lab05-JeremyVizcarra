using Lab05_JeremyVizcarra.Application.Models;
using Lab05_JeremyVizcarra.Domain.Entities;

namespace Lab05_JeremyVizcarra.Application.Ports.Out;

public interface ITokenService
{
    TokenModel Generar(Usuario usuario);
}
