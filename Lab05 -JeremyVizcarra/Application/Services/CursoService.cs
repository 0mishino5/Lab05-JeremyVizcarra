using Lab05_JeremyVizcarra.Application.Ports.In;
using Lab05_JeremyVizcarra.Application.Ports.Out;
using Lab05_JeremyVizcarra.Domain.Entities;
using Lab05_JeremyVizcarra.Domain.Exceptions;

namespace Lab05_JeremyVizcarra.Application.Services;

public class CursoService : ICursoService
{
    private readonly IUnitOfWork _unitOfWork;

    public CursoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Curso>> ObtenerTodos()
    {
        return await _unitOfWork.Cursos.ObtenerTodos();
    }

    public async Task<Curso> ObtenerPorId(int id)
    {
        Curso? curso = await _unitOfWork.Cursos.ObtenerPorId(id);

        if (curso == null)
        {
            throw new RecursoNoEncontradoException($"No existe el curso con Id {id}");
        }

        return curso;
    }

    public async Task<List<Estudiante>> ObtenerEstudiantes(int cursoId)
    {
        Curso curso = await ObtenerPorId(cursoId);

        return await _unitOfWork.Cursos.ObtenerEstudiantes(curso.CursoId);
    }

    public async Task<Curso> Crear(Curso curso)
    {
        Validar(curso);

        if (await _unitOfWork.Cursos.ObtenerPorNombre(curso.Nombre) != null)
        {
            throw new ReglaNegocioException($"Ya existe un curso con el nombre {curso.Nombre}");
        }

        curso.CursoId = 0;
        curso.Matriculas.Clear();
        curso.Materias.Clear();
        curso.Evaluaciones.Clear();
        curso.Asistencias.Clear();

        await _unitOfWork.Cursos.Agregar(curso);
        await _unitOfWork.GuardarCambios();

        return curso;
    }

    public async Task<Curso> Actualizar(int id, Curso datos)
    {
        Curso curso = await ObtenerPorId(id);

        Validar(datos);

        Curso? repetido = await _unitOfWork.Cursos.ObtenerPorNombre(datos.Nombre);

        if (repetido != null && repetido.CursoId != id)
        {
            throw new ReglaNegocioException($"Ya existe un curso con el nombre {datos.Nombre}");
        }

        curso.Nombre = datos.Nombre;
        curso.Descripcion = datos.Descripcion;
        curso.Creditos = datos.Creditos;

        _unitOfWork.Cursos.Actualizar(curso);
        await _unitOfWork.GuardarCambios();

        return curso;
    }

    public async Task Eliminar(int id)
    {
        Curso curso = await ObtenerPorId(id);

        if (curso.Matriculas.Count > 0)
        {
            throw new ReglaNegocioException($"No se puede eliminar el curso {curso.Nombre} porque tiene {curso.Matriculas.Count} estudiante(s) matriculado(s)");
        }

        foreach (Materia materia in curso.Materias.ToList())
        {
            _unitOfWork.Repositorio<Materia>().Eliminar(materia);
        }

        _unitOfWork.Cursos.Eliminar(curso);
        await _unitOfWork.GuardarCambios();
    }

    private void Validar(Curso curso)
    {
        if (string.IsNullOrWhiteSpace(curso.Nombre))
        {
            throw new ReglaNegocioException("El nombre del curso es obligatorio");
        }

        if (curso.Creditos < 1 || curso.Creditos > 10)
        {
            throw new ReglaNegocioException("Los creditos del curso deben estar entre 1 y 10");
        }
    }
}
