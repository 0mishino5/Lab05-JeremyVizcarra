using Lab05_JeremyVizcarra.Application.Ports.In;
using Lab05_JeremyVizcarra.Application.Ports.Out;
using Lab05_JeremyVizcarra.Domain.Entities;
using Lab05_JeremyVizcarra.Domain.Exceptions;

namespace Lab05_JeremyVizcarra.Application.Services;

public class EvaluacionService : IEvaluacionService
{
    private readonly IUnitOfWork _unitOfWork;

    public EvaluacionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Evaluacion>> ObtenerTodos()
    {
        return await _unitOfWork.Repositorio<Evaluacion>().ObtenerTodos();
    }

    public async Task<Evaluacion> ObtenerPorId(int id)
    {
        Evaluacion? evaluacion = await _unitOfWork.Repositorio<Evaluacion>().ObtenerPorId(id);

        if (evaluacion == null)
        {
            throw new RecursoNoEncontradoException($"No existe la evaluacion con Id {id}");
        }

        return evaluacion;
    }

    public async Task<Evaluacion> Crear(Evaluacion evaluacion)
    {
        await Validar(evaluacion);

        evaluacion.EvaluacionId = 0;
        evaluacion.Estudiante = null;
        evaluacion.Curso = null;

        await _unitOfWork.Repositorio<Evaluacion>().Agregar(evaluacion);
        await _unitOfWork.GuardarCambios();

        return evaluacion;
    }

    public async Task<Evaluacion> Actualizar(int id, Evaluacion datos)
    {
        Evaluacion evaluacion = await ObtenerPorId(id);

        await Validar(datos);

        evaluacion.EstudianteId = datos.EstudianteId;
        evaluacion.CursoId = datos.CursoId;
        evaluacion.Calificacion = datos.Calificacion;
        evaluacion.Fecha = datos.Fecha;

        _unitOfWork.Repositorio<Evaluacion>().Actualizar(evaluacion);
        await _unitOfWork.GuardarCambios();

        return evaluacion;
    }

    public async Task Eliminar(int id)
    {
        Evaluacion evaluacion = await ObtenerPorId(id);

        _unitOfWork.Repositorio<Evaluacion>().Eliminar(evaluacion);
        await _unitOfWork.GuardarCambios();
    }

    private async Task Validar(Evaluacion evaluacion)
    {
        if (evaluacion.EstudianteId == null || evaluacion.CursoId == null)
        {
            throw new ReglaNegocioException("La evaluacion debe indicar el estudiante y el curso");
        }

        if (evaluacion.Calificacion == null || evaluacion.Calificacion < 0 || evaluacion.Calificacion > 20)
        {
            throw new ReglaNegocioException("La calificacion debe estar entre 0 y 20");
        }

        if (evaluacion.Fecha == null)
        {
            throw new ReglaNegocioException("La fecha de la evaluacion es obligatoria");
        }

        if (!await _unitOfWork.Matriculas.EstaMatriculado(evaluacion.EstudianteId.Value, evaluacion.CursoId.Value))
        {
            throw new ReglaNegocioException($"El estudiante con Id {evaluacion.EstudianteId} no esta matriculado en el curso con Id {evaluacion.CursoId}");
        }
    }
}
