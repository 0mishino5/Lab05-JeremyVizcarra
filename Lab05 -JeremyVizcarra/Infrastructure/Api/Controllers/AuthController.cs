using System.Security.Claims;
using Lab05_JeremyVizcarra.Application.Models;
using Lab05_JeremyVizcarra.Application.Ports.In;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab05_JeremyVizcarra.Infrastructure.Api.Controllers;

[ApiController]
[Route("api/jvizcarra/auth")]
public class AuthController : ControllerBase
{
    private readonly IAutenticacionService _service;

    public AuthController(IAutenticacionService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel credenciales)
    {
        return Ok(await _service.Login(credenciales));
    }

    [Authorize]
    [HttpGet("perfil")]
    public IActionResult GetPerfil()
    {
        return Ok(new
        {
            id = User.FindFirstValue(ClaimTypes.NameIdentifier),
            username = User.FindFirstValue(ClaimTypes.Name),
            rol = User.FindFirstValue(ClaimTypes.Role),
            correo = User.FindFirstValue(ClaimTypes.Email)
        });
    }

    [Authorize(Policy = "Administrador")]
    [HttpGet("admin")]
    public IActionResult GetAdminData()
    {
        return Ok(new { message = "Datos solo para administradores" });
    }

    [Authorize(Policy = "GestionAcademica")]
    [HttpGet("docentes")]
    public IActionResult GetDocentesData()
    {
        return Ok(new { message = "Datos para administradores y docentes" });
    }

    [Authorize(Policy = "Consulta")]
    [HttpGet("consulta")]
    public IActionResult GetConsultaData()
    {
        return Ok(new { message = "Datos para cualquier usuario autenticado" });
    }
}
