using Lab05_JeremyVizcarra.Domain.Entities;

namespace Lab05_JeremyVizcarra.Application.Ports.In;

public interface ICursoService
{
    Task<List<Curso>> ObtenerTodos();
    Task<Curso> ObtenerPorId(int id);
    Task<List<Estudiante>> ObtenerEstudiantes(int cursoId);
    Task<Curso> Crear(Curso curso);
    Task<Curso> Actualizar(int id, Curso datos);
    Task Eliminar(int id);
}
