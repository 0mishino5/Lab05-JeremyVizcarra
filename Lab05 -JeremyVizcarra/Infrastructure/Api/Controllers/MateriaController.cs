using Lab05_JeremyVizcarra.Application.Ports.In;
using Lab05_JeremyVizcarra.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab05_JeremyVizcarra.Infrastructure.Api.Controllers;

[ApiController]
[Authorize(Policy = "Consulta")]
[Route("api/jvizcarra/materias")]
public class MateriaController : ControllerBase
{
    private readonly IMateriaService _service;

    public MateriaController(IMateriaService service)
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

    [Authorize(Policy = "GestionAcademica")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Materia datos)
    {
        Materia creado = await _service.Crear(datos);

        return CreatedAtAction(nameof(GetById), new { id = creado.MateriaId }, creado);
    }

    [Authorize(Policy = "GestionAcademica")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Materia datos)
    {
        return Ok(await _service.Actualizar(id, datos));
    }

    [Authorize(Policy = "Administrador")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Eliminar(id);

        return Ok(new { message = "Materia eliminada" });
    }
}
