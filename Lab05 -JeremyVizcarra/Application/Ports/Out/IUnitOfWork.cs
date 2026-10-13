namespace Lab05_JeremyVizcarra.Application.Ports.Out;

public interface IUnitOfWork : IDisposable
{
    IEstudianteRepository Estudiantes { get; }
    ICursoRepository Cursos { get; }
    IMatriculaRepository Matriculas { get; }
    IUsuarioRepository Usuarios { get; }

    IRepository<T> Repositorio<T>() where T : class;

    Task<int> GuardarCambios();
}
