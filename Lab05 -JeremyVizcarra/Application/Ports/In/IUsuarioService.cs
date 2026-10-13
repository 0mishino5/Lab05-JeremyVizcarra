using Lab05_JeremyVizcarra.Application.Models;
using Lab05_JeremyVizcarra.Domain.Entities;

namespace Lab05_JeremyVizcarra.Application.Ports.In;

public interface IUsuarioService
{
    Task<List<Usuario>> ObtenerTodos();
    Task<Usuario> ObtenerPorId(int id);
    Task<Usuario> Crear(RegistroModel datos);
    Task<Usuario> Actualizar(int id, RegistroModel datos);
    Task Eliminar(int id);
}
