using Lab05_JeremyVizcarra.Application.Ports.Out;
using Lab05_JeremyVizcarra.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lab05_JeremyVizcarra.Infrastructure.Persistence;

public class EstudianteRepository : Repository<Estudiante>, IEstudianteRepository
{
    public EstudianteRepository(Lab05DbContext context) : base(context)
    {
    }

    protected override IQueryable<Estudiante> Consulta()
    {
        return _dbSet
            .Include(e => e.Matriculas)
            .ThenInclude(m => m.Curso)
            .Include(e => e.Evaluaciones)
            .Include(e => e.Asistencias);
    }

    public async Task<Estudiante?> ObtenerPorCorreo(string correo)
    {
        return await Consulta()
            .FirstOrDefaultAsync(e => e.Correo != null && e.Correo.ToLower() == correo.ToLower());
    }
}
