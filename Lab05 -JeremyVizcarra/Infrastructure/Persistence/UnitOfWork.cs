using System.Collections;
using Lab05_JeremyVizcarra.Application.Ports.Out;

namespace Lab05_JeremyVizcarra.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly Lab05DbContext _context;
    private readonly Hashtable _repositorios;

    public IEstudianteRepository Estudiantes { get; }
    public ICursoRepository Cursos { get; }
    public IMatriculaRepository Matriculas { get; }
    public IUsuarioRepository Usuarios { get; }

    public UnitOfWork(Lab05DbContext context)
    {
        _context = context;
        _repositorios = new Hashtable();
        Estudiantes = new EstudianteRepository(context);
        Cursos = new CursoRepository(context);
        Matriculas = new MatriculaRepository(context);
        Usuarios = new UsuarioRepository(context);
    }

    public IRepository<T> Repositorio<T>() where T : class
    {
        string tipo = typeof(T).Name;

        if (_repositorios.ContainsKey(tipo))
        {
            return (IRepository<T>)_repositorios[tipo]!;
        }

        Type tipoRepositorio = typeof(GenericRepository<>);
        object? instancia = Activator.CreateInstance(tipoRepositorio.MakeGenericType(typeof(T)), _context);

        if (instancia == null)
        {
            throw new InvalidOperationException($"No se pudo crear el repositorio generico para el tipo {tipo}");
        }

        _repositorios.Add(tipo, instancia);

        return (IRepository<T>)instancia;
    }

    public async Task<int> GuardarCambios()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
