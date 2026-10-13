namespace Lab05_JeremyVizcarra.Application.Ports.Out;

public interface IRepository<T> where T : class
{
    Task<List<T>> ObtenerTodos();
    Task<T?> ObtenerPorId(int id);
    Task<bool> Existe(int id);
    Task Agregar(T entidad);
    Task AgregarRango(List<T> entidades);
    void Actualizar(T entidad);
    void Eliminar(T entidad);
}
