using Lab05_JeremyVizcarra.Domain.Entities;

namespace Lab05_JeremyVizcarra.Application.Ports.In;

public interface IEvaluacionService
{
    Task<List<Evaluacion>> ObtenerTodos();
    Task<Evaluacion> ObtenerPorId(int id);
    Task<Evaluacion> Crear(Evaluacion evaluacion);
    Task<Evaluacion> Actualizar(int id, Evaluacion datos);
    Task Eliminar(int id);
}
