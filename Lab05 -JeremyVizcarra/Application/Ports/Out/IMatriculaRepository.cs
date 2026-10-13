using Lab05_JeremyVizcarra.Domain.Entities;

namespace Lab05_JeremyVizcarra.Application.Ports.Out;

public interface IMatriculaRepository : IRepository<Matricula>
{
    Task<List<Matricula>> ObtenerPorEstudiante(int estudianteId);
    Task<List<Matricula>> ObtenerPorCurso(int cursoId);
    Task<List<Matricula>> ObtenerPorEstudianteYSemestre(int estudianteId, string semestre);
    Task<bool> EstaMatriculado(int estudianteId, int cursoId);
}
