using Lab05_JeremyVizcarra.Application.Ports.In;
using Lab05_JeremyVizcarra.Application.Ports.Out;
using Lab05_JeremyVizcarra.Domain.Entities;
using Lab05_JeremyVizcarra.Domain.Exceptions;

namespace Lab05_JeremyVizcarra.Application.Services;

public class EstudianteService : IEstudianteService
{
    private readonly IUnitOfWork _unitOfWork;

    public EstudianteService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Estudiante>> ObtenerTodos()
    {
        return await _unitOfWork.Estudiantes.ObtenerTodos();
    }

    public async Task<Estudiante> ObtenerPorId(int id)
    {
        Estudiante? estudiante = await _unitOfWork.Estudiantes.ObtenerPorId(id);

        if (estudiante == null)
        {
            throw new RecursoNoEncontradoException($"No existe el estudiante con Id {id}");
        }

        return estudiante;
    }

    public async Task<Estudiante> ObtenerPorCorreo(string correo)
    {
        Estudiante? estudiante = await _unitOfWork.Estudiantes.ObtenerPorCorreo(correo);

        if (estudiante == null)
        {
            throw new RecursoNoEncontradoException($"No existe el estudiante con el correo {correo}");
        }

        return estudiante;
    }

    public async Task<Estudiante> Crear(Estudiante estudiante)
    {
        Validar(estudiante);

        if (estudiante.Correo != null && await _unitOfWork.Estudiantes.ObtenerPorCorreo(estudiante.Correo) != null)
        {
            throw new ReglaNegocioException($"Ya existe un estudiante con el correo {estudiante.Correo}");
        }

        estudiante.EstudianteId = 0;
        estudiante.Matriculas.Clear();
        estudiante.Evaluaciones.Clear();
        estudiante.Asistencias.Clear();

        await _unitOfWork.Estudiantes.Agregar(estudiante);
        await _unitOfWork.GuardarCambios();

        return estudiante;
    }

    public async Task<Estudiante> Actualizar(int id, Estudiante datos)
    {
        Estudiante estudiante = await ObtenerPorId(id);

        Validar(datos);

        if (datos.Correo != null)
        {
            Estudiante? repetido = await _unitOfWork.Estudiantes.ObtenerPorCorreo(datos.Correo);

            if (repetido != null && repetido.EstudianteId != id)
            {
                throw new ReglaNegocioException($"Ya existe un estudiante con el correo {datos.Correo}");
            }
        }

        estudiante.Nombre = datos.Nombre;
        estudiante.Edad = datos.Edad;
        estudiante.Direccion = datos.Direccion;
        estudiante.Telefono = datos.Telefono;
        estudiante.Correo = datos.Correo;

        _unitOfWork.Estudiantes.Actualizar(estudiante);
        await _unitOfWork.GuardarCambios();

        return estudiante;
    }

    public async Task Eliminar(int id)
    {
        Estudiante estudiante = await ObtenerPorId(id);

        foreach (Evaluacion evaluacion in estudiante.Evaluaciones.ToList())
        {
            _unitOfWork.Repositorio<Evaluacion>().Eliminar(evaluacion);
        }

        foreach (Asistencia asistencia in estudiante.Asistencias.ToList())
        {
            _unitOfWork.Repositorio<Asistencia>().Eliminar(asistencia);
        }

        foreach (Matricula matricula in estudiante.Matriculas.ToList())
        {
            _unitOfWork.Matriculas.Eliminar(matricula);
        }

        _unitOfWork.Estudiantes.Eliminar(estudiante);
        await _unitOfWork.GuardarCambios();
    }

    private void Validar(Estudiante estudiante)
    {
        if (string.IsNullOrWhiteSpace(estudiante.Nombre))
        {
            throw new ReglaNegocioException("El nombre del estudiante es obligatorio");
        }

        if (estudiante.Edad < 16 || estudiante.Edad > 90)
        {
            throw new ReglaNegocioException("La edad del estudiante debe estar entre 16 y 90 años");
        }

        if (estudiante.Correo != null && !estudiante.Correo.Contains('@'))
        {
            throw new ReglaNegocioException("El correo del estudiante no tiene un formato valido");
        }
    }
}
