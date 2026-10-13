using Lab05_JeremyVizcarra.Application.Ports.In;
using Lab05_JeremyVizcarra.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab05_JeremyVizcarra.Infrastructure.Api.Controllers;

[ApiController]
[Authorize(Policy = "Consulta")]
[Route("api/jvizcarra/matriculas")]
public class MatriculaController : ControllerBase
{
    private readonly IMatriculaService _service;

    public MatriculaController(IMatriculaService service)
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

    [HttpGet("estudiante/{estudianteId:int}")]
    public async Task<IActionResult> GetByEstudiante(int estudianteId)
    {
        return Ok(await _service.ObtenerPorEstudiante(estudianteId));
    }

    [HttpGet("curso/{cursoId:int}")]
    public async Task<IActionResult> GetByCurso(int cursoId)
    {
        return Ok(await _service.ObtenerPorCurso(cursoId));
    }

    [Authorize(Policy = "GestionAcademica")]
    [HttpPost("curso/{cursoId:int}")]
    public async Task<IActionResult> CreateByCurso(int cursoId, [FromQuery] string semestre, [FromBody] List<int> estudianteIds)
    {
        return Ok(await _service.MatricularEnCurso(cursoId, semestre, estudianteIds));
    }

    [Authorize(Policy = "GestionAcademica")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Matricula datos)
    {
        Matricula creado = await _service.Crear(datos);

        return CreatedAtAction(nameof(GetById), new { id = creado.MatriculaId }, creado);
    }

    [Authorize(Policy = "GestionAcademica")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Matricula datos)
    {
        return Ok(await _service.Actualizar(id, datos));
    }

    [Authorize(Policy = "Administrador")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Eliminar(id);

        return Ok(new { message = "Matricula eliminada junto con las evaluaciones y asistencias del estudiante en ese curso" });
    }
}
