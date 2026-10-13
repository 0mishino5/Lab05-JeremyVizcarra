using Lab05_JeremyVizcarra.Domain.Entities;

namespace Lab05_JeremyVizcarra.Application.Ports.Out;

public interface ICursoRepository : IRepository<Curso>
{
    Task<Curso?> ObtenerPorNombre(string nombre);
    Task<List<Estudiante>> ObtenerEstudiantes(int cursoId);
}
