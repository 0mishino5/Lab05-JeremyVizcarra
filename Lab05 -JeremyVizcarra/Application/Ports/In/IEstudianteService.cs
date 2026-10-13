using Lab05_JeremyVizcarra.Domain.Entities;

namespace Lab05_JeremyVizcarra.Application.Ports.In;

public interface IEstudianteService
{
    Task<List<Estudiante>> ObtenerTodos();
    Task<Estudiante> ObtenerPorId(int id);
    Task<Estudiante> ObtenerPorCorreo(string correo);
    Task<Estudiante> Crear(Estudiante estudiante);
    Task<Estudiante> Actualizar(int id, Estudiante datos);
    Task Eliminar(int id);
}
