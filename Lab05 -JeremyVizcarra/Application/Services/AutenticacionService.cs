using Lab05_JeremyVizcarra.Application.Models;
using Lab05_JeremyVizcarra.Application.Ports.In;
using Lab05_JeremyVizcarra.Application.Ports.Out;
using Lab05_JeremyVizcarra.Domain.Entities;
using Lab05_JeremyVizcarra.Domain.Exceptions;

namespace Lab05_JeremyVizcarra.Application.Services;

public class AutenticacionService : IAutenticacionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashService _hashService;
    private readonly ITokenService _tokenService;

    public AutenticacionService(IUnitOfWork unitOfWork, IHashService hashService, ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _hashService = hashService;
        _tokenService = tokenService;
    }

    public async Task<TokenModel> Login(LoginModel credenciales)
    {
        if (string.IsNullOrWhiteSpace(credenciales.Username) || string.IsNullOrWhiteSpace(credenciales.Password))
        {
            throw new ReglaNegocioException("El usuario y la contrasena son obligatorios");
        }

        Usuario? usuario = await _unitOfWork.Usuarios.ObtenerPorUsername(credenciales.Username);

        if (usuario == null || !_hashService.Verificar(credenciales.Password, usuario.PasswordHash))
        {
            throw new CredencialesInvalidasException("Usuario o contrasena incorrectos");
        }

        if (!usuario.Activo)
        {
            throw new CredencialesInvalidasException($"El usuario {usuario.Username} esta desactivado");
        }

        return _tokenService.Generar(usuario);
    }
}
