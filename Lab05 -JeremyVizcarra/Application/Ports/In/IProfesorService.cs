using Lab05_JeremyVizcarra.Domain.Entities;

namespace Lab05_JeremyVizcarra.Application.Ports.In;

public interface IProfesorService
{
    Task<List<Profesor>> ObtenerTodos();
    Task<Profesor> ObtenerPorId(int id);
    Task<Profesor> Crear(Profesor profesor);
    Task<Profesor> Actualizar(int id, Profesor datos);
    Task Eliminar(int id);
}
