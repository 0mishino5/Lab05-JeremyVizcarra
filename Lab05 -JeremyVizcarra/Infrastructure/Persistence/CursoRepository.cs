using Lab05_JeremyVizcarra.Application.Ports.Out;
using Lab05_JeremyVizcarra.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lab05_JeremyVizcarra.Infrastructure.Persistence;

public class CursoRepository : Repository<Curso>, ICursoRepository
{
    public CursoRepository(Lab05DbContext context) : base(context)
    {
    }

    protected override IQueryable<Curso> Consulta()
    {
        return _dbSet
            .Include(c => c.Materias)
            .Include(c => c.Matriculas)
            .ThenInclude(m => m.Estudiante);
    }

    public async Task<Curso?> ObtenerPorNombre(string nombre)
    {
        return await Consulta()
            .FirstOrDefaultAsync(c => c.Nombre.ToLower() == nombre.ToLower());
    }

    public async Task<List<Estudiante>> ObtenerEstudiantes(int cursoId)
    {
        return await _dbSet
            .Where(c => c.CursoId == cursoId)
            .SelectMany(c => c.Matriculas)
            .Where(m => m.Estudiante != null)
            .Select(m => m.Estudiante!)
            .Distinct()
            .OrderBy(e => e.EstudianteId)
            .ToListAsync();
    }
}
