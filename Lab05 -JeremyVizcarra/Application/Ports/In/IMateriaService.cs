using Lab05_JeremyVizcarra.Domain.Entities;

namespace Lab05_JeremyVizcarra.Application.Ports.In;

public interface IMateriaService
{
    Task<List<Materia>> ObtenerTodos();
    Task<Materia> ObtenerPorId(int id);
    Task<Materia> Crear(Materia materia);
    Task<Materia> Actualizar(int id, Materia datos);
    Task Eliminar(int id);
}
