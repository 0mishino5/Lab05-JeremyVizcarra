using Lab05_JeremyVizcarra.Domain.Entities;

namespace Lab05_JeremyVizcarra.Application.Ports.Out;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> ObtenerPorUsername(string username);
    Task<bool> ExisteUsername(string username);
}
