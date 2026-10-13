using Lab05_JeremyVizcarra.Application.Ports.In;
using Lab05_JeremyVizcarra.Application.Ports.Out;
using Lab05_JeremyVizcarra.Domain.Entities;
using Lab05_JeremyVizcarra.Domain.Exceptions;

namespace Lab05_JeremyVizcarra.Application.Services;

public class MateriaService : IMateriaService
{
    private readonly IUnitOfWork _unitOfWork;

    public MateriaService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Materia>> ObtenerTodos()
    {
        return await _unitOfWork.Repositorio<Materia>().ObtenerTodos();
    }

    public async Task<Materia> ObtenerPorId(int id)
    {
        Materia? materia = await _unitOfWork.Repositorio<Materia>().ObtenerPorId(id);

        if (materia == null)
        {
            throw new RecursoNoEncontradoException($"No existe la materia con Id {id}");
        }

        return materia;
    }

    public async Task<Materia> Crear(Materia materia)
    {
        await Validar(materia, 0);

        materia.MateriaId = 0;
        materia.Curso = null;

        await _unitOfWork.Repositorio<Materia>().Agregar(materia);
        await _unitOfWork.GuardarCambios();

        return materia;
    }

    public async Task<Materia> Actualizar(int id, Materia datos)
    {
        Materia materia = await ObtenerPorId(id);

        await Validar(datos, id);

        materia.CursoId = datos.CursoId;
        materia.Nombre = datos.Nombre;
        materia.Descripcion = datos.Descripcion;

        _unitOfWork.Repositorio<Materia>().Actualizar(materia);
        await _unitOfWork.GuardarCambios();

        return materia;
    }

    public async Task Eliminar(int id)
    {
        Materia materia = await ObtenerPorId(id);

        _unitOfWork.Repositorio<Materia>().Eliminar(materia);
        await _unitOfWork.GuardarCambios();
    }

    private async Task Validar(Materia materia, int id)
    {
        if (string.IsNullOrWhiteSpace(materia.Nombre))
        {
            throw new ReglaNegocioException("El nombre de la materia es obligatorio");
        }

        if (materia.CursoId == null || !await _unitOfWork.Cursos.Existe(materia.CursoId.Value))
        {
            throw new ReglaNegocioException($"No existe el curso con Id {materia.CursoId}");
        }

        List<Materia> materias = await _unitOfWork.Repositorio<Materia>().ObtenerTodos();

        if (materias.Any(m => m.MateriaId != id && m.CursoId == materia.CursoId && m.Nombre.ToLower() == materia.Nombre.ToLower()))
        {
            throw new ReglaNegocioException($"El curso ya tiene una materia llamada {materia.Nombre}");
        }
    }
}
