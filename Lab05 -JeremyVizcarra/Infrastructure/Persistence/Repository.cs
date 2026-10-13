using Lab05_JeremyVizcarra.Application.Ports.Out;
using Microsoft.EntityFrameworkCore;

namespace Lab05_JeremyVizcarra.Infrastructure.Persistence;

public abstract class Repository<T> : IRepository<T> where T : class
{
    protected readonly Lab05DbContext _context;
    protected readonly DbSet<T> _dbSet;

    protected Repository(Lab05DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<List<T>> ObtenerTodos()
    {
        string clave = NombreDeClave();

        return await Consulta()
            .OrderBy(e => EF.Property<int>(e, clave))
            .ToListAsync();
    }

    public virtual async Task<T?> ObtenerPorId(int id)
    {
        string clave = NombreDeClave();

        return await Consulta()
            .FirstOrDefaultAsync(e => EF.Property<int>(e, clave) == id);
    }

    public virtual async Task<bool> Existe(int id)
    {
        string clave = NombreDeClave();

        return await _dbSet.AnyAsync(e => EF.Property<int>(e, clave) == id);
    }

    public virtual async Task Agregar(T entidad)
    {
        await _dbSet.AddAsync(entidad);
    }

    public virtual async Task AgregarRango(List<T> entidades)
    {
        await _dbSet.AddRangeAsync(entidades);
    }

    public virtual void Actualizar(T entidad)
    {
        _dbSet.Update(entidad);
    }

    public virtual void Eliminar(T entidad)
    {
        _dbSet.Remove(entidad);
    }

    protected virtual IQueryable<T> Consulta()
    {
        return _dbSet;
    }

    protected string NombreDeClave()
    {
        return _context.Model
            .FindEntityType(typeof(T))!
            .FindPrimaryKey()!
            .Properties[0]
            .Name;
    }
}
