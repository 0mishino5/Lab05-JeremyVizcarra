using Lab05_JeremyVizcarra.Application.Models;
using Lab05_JeremyVizcarra.Application.Ports.In;
using Lab05_JeremyVizcarra.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab05_JeremyVizcarra.Infrastructure.Api.Controllers;

[ApiController]
[Authorize(Policy = "Administrador")]
[Route("api/jvizcarra/usuarios")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _service;

    public UsuarioController(IUsuarioService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.ObtenerTodos());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _service.ObtenerPorId(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RegistroModel datos)
    {
        Usuario creado = await _service.Crear(datos);

        return CreatedAtAction(nameof(GetById), new { id = creado.UsuarioId }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] RegistroModel datos)
    {
        return Ok(await _service.Actualizar(id, datos));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Eliminar(id);

        return Ok(new { message = "Usuario eliminado" });
    }
}
