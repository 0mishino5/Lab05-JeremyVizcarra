using Lab05_JeremyVizcarra.Application.Ports.In;
using Lab05_JeremyVizcarra.Application.Ports.Out;
using Lab05_JeremyVizcarra.Domain.Entities;
using Lab05_JeremyVizcarra.Domain.Exceptions;

namespace Lab05_JeremyVizcarra.Application.Services;

public class ProfesorService : IProfesorService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProfesorService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Profesor>> ObtenerTodos()
    {
        return await _unitOfWork.Repositorio<Profesor>().ObtenerTodos();
    }

    public async Task<Profesor> ObtenerPorId(int id)
    {
        Profesor? profesor = await _unitOfWork.Repositorio<Profesor>().ObtenerPorId(id);

        if (profesor == null)
        {
            throw new RecursoNoEncontradoException($"No existe el profesor con Id {id}");
        }

        return profesor;
    }

    public async Task<Profesor> Crear(Profesor profesor)
    {
        Validar(profesor);

        await ValidarCorreoUnico(profesor.Correo, 0);

        profesor.ProfesorId = 0;

        await _unitOfWork.Repositorio<Profesor>().Agregar(profesor);
        await _unitOfWork.GuardarCambios();

        return profesor;
    }

    public async Task<Profesor> Actualizar(int id, Profesor datos)
    {
        Profesor profesor = await ObtenerPorId(id);

        Validar(datos);

        await ValidarCorreoUnico(datos.Correo, id);

        profesor.Nombre = datos.Nombre;
        profesor.Especialidad = datos.Especialidad;
        profesor.Correo = datos.Correo;

        _unitOfWork.Repositorio<Profesor>().Actualizar(profesor);
        await _unitOfWork.GuardarCambios();

        return profesor;
    }

    public async Task Eliminar(int id)
    {
        Profesor profesor = await ObtenerPorId(id);

        _unitOfWork.Repositorio<Profesor>().Eliminar(profesor);
        await _unitOfWork.GuardarCambios();
    }

    private void Validar(Profesor profesor)
    {
        if (string.IsNullOrWhiteSpace(profesor.Nombre))
        {
            throw new ReglaNegocioException("El nombre del profesor es obligatorio");
        }

        if (string.IsNullOrWhiteSpace(profesor.Especialidad))
        {
            throw new ReglaNegocioException("La especialidad del profesor es obligatoria");
        }

        if (profesor.Correo != null && !profesor.Correo.Contains('@'))
        {
            throw new ReglaNegocioException("El correo del profesor no tiene un formato valido");
        }
    }

    private async Task ValidarCorreoUnico(string? correo, int id)
    {
        if (string.IsNullOrWhiteSpace(correo))
        {
            return;
        }

        List<Profesor> profesores = await _unitOfWork.Repositorio<Profesor>().ObtenerTodos();

        if (profesores.Any(p => p.ProfesorId != id && p.Correo != null && p.Correo.ToLower() == correo.ToLower()))
        {
            throw new ReglaNegocioException($"Ya existe un profesor con el correo {correo}");
        }
    }
}
