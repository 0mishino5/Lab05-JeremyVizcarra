using Lab05_JeremyVizcarra.Application.Ports.In;
using Lab05_JeremyVizcarra.Application.Ports.Out;
using Lab05_JeremyVizcarra.Domain.Entities;
using Lab05_JeremyVizcarra.Domain.Exceptions;

namespace Lab05_JeremyVizcarra.Application.Services;

public class AsistenciaService : IAsistenciaService
{
    private static readonly string[] EstadosValidos = { "Presente", "Ausente", "Justificada" };

    private readonly IUnitOfWork _unitOfWork;

    public AsistenciaService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Asistencia>> ObtenerTodos()
    {
        return await _unitOfWork.Repositorio<Asistencia>().ObtenerTodos();
    }

    public async Task<Asistencia> ObtenerPorId(int id)
    {
        Asistencia? asistencia = await _unitOfWork.Repositorio<Asistencia>().ObtenerPorId(id);

        if (asistencia == null)
        {
            throw new RecursoNoEncontradoException($"No existe la asistencia con Id {id}");
        }

        return asistencia;
    }

    public async Task<Asistencia> Crear(Asistencia asistencia)
    {
        await Validar(asistencia, 0);

        asistencia.AsistenciaId = 0;
        asistencia.Estudiante = null;
        asistencia.Curso = null;

        await _unitOfWork.Repositorio<Asistencia>().Agregar(asistencia);
        await _unitOfWork.GuardarCambios();

        return asistencia;
    }

    public async Task<Asistencia> Actualizar(int id, Asistencia datos)
    {
        Asistencia asistencia = await ObtenerPorId(id);

        await Validar(datos, id);

        asistencia.EstudianteId = datos.EstudianteId;
        asistencia.CursoId = datos.CursoId;
        asistencia.Fecha = datos.Fecha;
        asistencia.Estado = datos.Estado;

        _unitOfWork.Repositorio<Asistencia>().Actualizar(asistencia);
        await _unitOfWork.GuardarCambios();

        return asistencia;
    }

    public async Task Eliminar(int id)
    {
        Asistencia asistencia = await ObtenerPorId(id);

        _unitOfWork.Repositorio<Asistencia>().Eliminar(asistencia);
        await _unitOfWork.GuardarCambios();
    }

    private async Task Validar(Asistencia asistencia, int id)
    {
        if (asistencia.EstudianteId == null || asistencia.CursoId == null)
        {
            throw new ReglaNegocioException("La asistencia debe indicar el estudiante y el curso");
        }

        if (asistencia.Fecha == null)
        {
            throw new ReglaNegocioException("La fecha de la asistencia es obligatoria");
        }

        if (asistencia.Estado == null || !EstadosValidos.Contains(asistencia.Estado))
        {
            throw new ReglaNegocioException($"El estado debe ser uno de: {string.Join(", ", EstadosValidos)}");
        }

        if (!await _unitOfWork.Matriculas.EstaMatriculado(asistencia.EstudianteId.Value, asistencia.CursoId.Value))
        {
            throw new ReglaNegocioException($"El estudiante con Id {asistencia.EstudianteId} no esta matriculado en el curso con Id {asistencia.CursoId}");
        }

        List<Asistencia> asistencias = await _unitOfWork.Repositorio<Asistencia>().ObtenerTodos();

        if (asistencias.Any(a => a.AsistenciaId != id && a.EstudianteId == asistencia.EstudianteId && a.CursoId == asistencia.CursoId && a.Fecha == asistencia.Fecha))
        {
            throw new ReglaNegocioException("Ya se registro la asistencia de ese estudiante en ese curso para esa fecha");
        }
    }
}
