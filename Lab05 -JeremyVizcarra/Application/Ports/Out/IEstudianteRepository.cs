using Lab05_JeremyVizcarra.Domain.Entities;

namespace Lab05_JeremyVizcarra.Application.Ports.Out;

public interface IEstudianteRepository : IRepository<Estudiante>
{
    Task<Estudiante?> ObtenerPorCorreo(string correo);
}
