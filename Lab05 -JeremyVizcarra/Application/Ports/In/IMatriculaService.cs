using Lab05_JeremyVizcarra.Domain.Entities;

namespace Lab05_JeremyVizcarra.Application.Ports.In;

public interface IMatriculaService
{
    Task<List<Matricula>> ObtenerTodos();
    Task<Matricula> ObtenerPorId(int id);
    Task<List<Matricula>> ObtenerPorEstudiante(int estudianteId);
    Task<List<Matricula>> ObtenerPorCurso(int cursoId);
    Task<Matricula> Crear(Matricula matricula);
    Task<List<Matricula>> MatricularEnCurso(int cursoId, string semestre, List<int> estudianteIds);
    Task<Matricula> Actualizar(int id, Matricula datos);
    Task Eliminar(int id);
}
