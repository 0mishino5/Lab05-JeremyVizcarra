using Lab05_JeremyVizcarra.Application.Ports.In;
using Lab05_JeremyVizcarra.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab05_JeremyVizcarra.Infrastructure.Api.Controllers;

[ApiController]
[Authorize(Policy = "Consulta")]
[Route("api/jvizcarra/cursos")]
public class CursoController : ControllerBase
{
    private readonly ICursoService _service;

    public CursoController(ICursoService service)
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

    [HttpGet("{id:int}/estudiantes")]
    public async Task<IActionResult> GetEstudiantes(int id)
    {
        return Ok(await _service.ObtenerEstudiantes(id));
    }

    [Authorize(Policy = "GestionAcademica")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Curso datos)
    {
        Curso creado = await _service.Crear(datos);

        return CreatedAtAction(nameof(GetById), new { id = creado.CursoId }, creado);
    }

    [Authorize(Policy = "GestionAcademica")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Curso datos)
    {
        return Ok(await _service.Actualizar(id, datos));
    }

    [Authorize(Policy = "Administrador")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Eliminar(id);

        return Ok(new { message = "Curso eliminado junto con sus materias" });
    }
}
