using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lab05_JeremyVizcarra.Application.Models;
using Lab05_JeremyVizcarra.Application.Ports.Out;
using Lab05_JeremyVizcarra.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Lab05_JeremyVizcarra.Infrastructure.Security;

public class JwtTokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public TokenModel Generar(Usuario usuario)
    {
        int minutos = int.Parse(_configuration["Jwt:MinutosDeExpiracion"]!);
        DateTime expira = DateTime.UtcNow.AddMinutes(minutos);

        Claim[] claims =
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
            new Claim(ClaimTypes.Name, usuario.Username),
            new Claim(ClaimTypes.Role, usuario.Rol),
            new Claim(ClaimTypes.Email, usuario.Correo ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        SymmetricSecurityKey clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
        SigningCredentials credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expira,
            signingCredentials: credenciales);

        return new TokenModel
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expira = expira,
            Username = usuario.Username,
            Rol = usuario.Rol
        };
    }
}
