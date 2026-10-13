using Lab05_JeremyVizcarra.Application.Ports.Out;
using Lab05_JeremyVizcarra.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lab05_JeremyVizcarra.Infrastructure.Persistence;

public class MatriculaRepository : Repository<Matricula>, IMatriculaRepository
{
    public MatriculaRepository(Lab05DbContext context) : base(context)
    {
    }

    protected override IQueryable<Matricula> Consulta()
    {
        return _dbSet
            .Include(m => m.Estudiante)
            .Include(m => m.Curso);
    }

    public async Task<List<Matricula>> ObtenerPorEstudiante(int estudianteId)
    {
        return await Consulta()
            .Where(m => m.EstudianteId == estudianteId)
            .OrderBy(m => m.MatriculaId)
            .ToListAsync();
    }

    public async Task<List<Matricula>> ObtenerPorCurso(int cursoId)
    {
        return await Consulta()
            .Where(m => m.CursoId == cursoId)
            .OrderBy(m => m.MatriculaId)
            .ToListAsync();
    }

    public async Task<List<Matricula>> ObtenerPorEstudianteYSemestre(int estudianteId, string semestre)
    {
        return await Consulta()
            .Where(m => m.EstudianteId == estudianteId && m.Semestre == semestre)
            .OrderBy(m => m.MatriculaId)
            .ToListAsync();
    }

    public async Task<bool> EstaMatriculado(int estudianteId, int cursoId)
    {
        return await _dbSet
            .AnyAsync(m => m.EstudianteId == estudianteId && m.CursoId == cursoId);
    }
}
