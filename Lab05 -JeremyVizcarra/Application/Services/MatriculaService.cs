using Lab05_JeremyVizcarra.Application.Ports.In;
using Lab05_JeremyVizcarra.Application.Ports.Out;
using Lab05_JeremyVizcarra.Domain.Entities;
using Lab05_JeremyVizcarra.Domain.Exceptions;

namespace Lab05_JeremyVizcarra.Application.Services;

public class MatriculaService : IMatriculaService
{
    private const int CreditosMaximosPorSemestre = 20;

    private readonly IUnitOfWork _unitOfWork;

    public MatriculaService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Matricula>> ObtenerTodos()
    {
        return await _unitOfWork.Matriculas.ObtenerTodos();
    }

    public async Task<Matricula> ObtenerPorId(int id)
    {
        Matricula? matricula = await _unitOfWork.Matriculas.ObtenerPorId(id);

        if (matricula == null)
        {
            throw new RecursoNoEncontradoException($"No existe la matricula con Id {id}");
        }

        return matricula;
    }

    public async Task<List<Matricula>> ObtenerPorEstudiante(int estudianteId)
    {
        return await _unitOfWork.Matriculas.ObtenerPorEstudiante(estudianteId);
    }

    public async Task<List<Matricula>> ObtenerPorCurso(int cursoId)
    {
        return await _unitOfWork.Matriculas.ObtenerPorCurso(cursoId);
    }

    public async Task<Matricula> Crear(Matricula matricula)
    {
        if (matricula.EstudianteId == null || matricula.CursoId == null)
        {
            throw new ReglaNegocioException("La matricula debe indicar el estudiante y el curso");
        }

        if (string.IsNullOrWhiteSpace(matricula.Semestre))
        {
            throw new ReglaNegocioException("El semestre de la matricula es obligatorio");
        }

        Curso curso = await ValidarCurso(matricula.CursoId.Value);

        await ValidarEstudiante(matricula.EstudianteId.Value, curso, matricula.Semestre);

        Matricula nueva = new Matricula
        {
            EstudianteId = matricula.EstudianteId,
            CursoId = matricula.CursoId,
            Semestre = matricula.Semestre
        };

        await _unitOfWork.Matriculas.Agregar(nueva);
        await _unitOfWork.GuardarCambios();

        return await ObtenerPorId(nueva.MatriculaId);
    }

    public async Task<List<Matricula>> MatricularEnCurso(int cursoId, string semestre, List<int> estudianteIds)
    {
        if (string.IsNullOrWhiteSpace(semestre))
        {
            throw new ReglaNegocioException("El semestre de la matricula es obligatorio");
        }

        if (estudianteIds.Count == 0)
        {
            throw new ReglaNegocioException("Debe indicar al menos un estudiante para matricular");
        }

        Curso curso = await ValidarCurso(cursoId);

        List<Matricula> nuevas = new List<Matricula>();

        foreach (int estudianteId in estudianteIds.Distinct())
        {
            await ValidarEstudiante(estudianteId, curso, semestre);

            nuevas.Add(new Matricula
            {
                EstudianteId = estudianteId,
                CursoId = cursoId,
                Semestre = semestre
            });
        }

        await _unitOfWork.Matriculas.AgregarRango(nuevas);
        await _unitOfWork.GuardarCambios();

        return await _unitOfWork.Matriculas.ObtenerPorCurso(cursoId);
    }

    public async Task<Matricula> Actualizar(int id, Matricula datos)
    {
        Matricula matricula = await ObtenerPorId(id);

        if (datos.EstudianteId == null || datos.CursoId == null)
        {
            throw new ReglaNegocioException("La matricula debe indicar el estudiante y el curso");
        }

        if (string.IsNullOrWhiteSpace(datos.Semestre))
        {
            throw new ReglaNegocioException("El semestre de la matricula es obligatorio");
        }

        Curso curso = await ValidarCurso(datos.CursoId.Value);

        await ValidarEstudiante(datos.EstudianteId.Value, curso, datos.Semestre, id);

        matricula.EstudianteId = datos.EstudianteId;
        matricula.CursoId = datos.CursoId;
        matricula.Semestre = datos.Semestre;

        _unitOfWork.Matriculas.Actualizar(matricula);
        await _unitOfWork.GuardarCambios();

        return matricula;
    }

    public async Task Eliminar(int id)
    {
        Matricula matricula = await ObtenerPorId(id);

        List<Evaluacion> evaluaciones = await _unitOfWork.Repositorio<Evaluacion>().ObtenerTodos();
        List<Asistencia> asistencias = await _unitOfWork.Repositorio<Asistencia>().ObtenerTodos();

        foreach (Evaluacion evaluacion in evaluaciones.Where(e => e.EstudianteId == matricula.EstudianteId && e.CursoId == matricula.CursoId))
        {
            _unitOfWork.Repositorio<Evaluacion>().Eliminar(evaluacion);
        }

        foreach (Asistencia asistencia in asistencias.Where(a => a.EstudianteId == matricula.EstudianteId && a.CursoId == matricula.CursoId))
        {
            _unitOfWork.Repositorio<Asistencia>().Eliminar(asistencia);
        }

        _unitOfWork.Matriculas.Eliminar(matricula);
        await _unitOfWork.GuardarCambios();
    }

    private async Task<Curso> ValidarCurso(int cursoId)
    {
        Curso? curso = await _unitOfWork.Cursos.ObtenerPorId(cursoId);

        if (curso == null)
        {
            throw new ReglaNegocioException($"No existe el curso con Id {cursoId}");
        }

        return curso;
    }

    private async Task ValidarEstudiante(int estudianteId, Curso curso, string semestre, int matriculaId = 0)
    {
        Estudiante? estudiante = await _unitOfWork.Estudiantes.ObtenerPorId(estudianteId);

        if (estudiante == null)
        {
            throw new ReglaNegocioException($"No existe el estudiante con Id {estudianteId}");
        }

        List<Matricula> delSemestre = await _unitOfWork.Matriculas.ObtenerPorEstudianteYSemestre(estudianteId, semestre);

        if (delSemestre.Any(m => m.CursoId == curso.CursoId && m.MatriculaId != matriculaId))
        {
            throw new ReglaNegocioException($"El estudiante {estudiante.Nombre} ya esta matriculado en el curso {curso.Nombre} en el semestre {semestre}");
        }

        int creditos = delSemestre
            .Where(m => m.MatriculaId != matriculaId && m.Curso != null)
            .Sum(m => m.Curso!.Creditos);

        if (creditos + curso.Creditos > CreditosMaximosPorSemestre)
        {
            throw new ReglaNegocioException($"El estudiante {estudiante.Nombre} superaria los {CreditosMaximosPorSemestre} creditos permitidos en el semestre {semestre}");
        }
    }
}
