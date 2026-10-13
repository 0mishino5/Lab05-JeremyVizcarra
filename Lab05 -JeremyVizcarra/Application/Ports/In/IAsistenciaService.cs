using Lab05_JeremyVizcarra.Domain.Entities;

namespace Lab05_JeremyVizcarra.Application.Ports.In;

public interface IAsistenciaService
{
    Task<List<Asistencia>> ObtenerTodos();
    Task<Asistencia> ObtenerPorId(int id);
    Task<Asistencia> Crear(Asistencia asistencia);
    Task<Asistencia> Actualizar(int id, Asistencia datos);
    Task Eliminar(int id);
}
