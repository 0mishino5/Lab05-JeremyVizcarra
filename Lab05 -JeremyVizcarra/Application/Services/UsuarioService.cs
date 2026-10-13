using Lab05_JeremyVizcarra.Application.Models;
using Lab05_JeremyVizcarra.Application.Ports.In;
using Lab05_JeremyVizcarra.Application.Ports.Out;
using Lab05_JeremyVizcarra.Domain.Entities;
using Lab05_JeremyVizcarra.Domain.Exceptions;

namespace Lab05_JeremyVizcarra.Application.Services;

public class UsuarioService : IUsuarioService
{
    private static readonly string[] RolesValidos = { "Admin", "Docente", "Estudiante" };

    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashService _hashService;

    public UsuarioService(IUnitOfWork unitOfWork, IHashService hashService)
    {
        _unitOfWork = unitOfWork;
        _hashService = hashService;
    }

    public async Task<List<Usuario>> ObtenerTodos()
    {
        return await _unitOfWork.Usuarios.ObtenerTodos();
    }

    public async Task<Usuario> ObtenerPorId(int id)
    {
        Usuario? usuario = await _unitOfWork.Usuarios.ObtenerPorId(id);

        if (usuario == null)
        {
            throw new RecursoNoEncontradoException($"No existe el usuario con Id {id}");
        }

        return usuario;
    }

    public async Task<Usuario> Crear(RegistroModel datos)
    {
        Validar(datos, true);

        if (await _unitOfWork.Usuarios.ExisteUsername(datos.Username))
        {
            throw new ReglaNegocioException($"Ya existe un usuario con el username {datos.Username}");
        }

        Usuario usuario = new Usuario
        {
            Username = datos.Username,
            PasswordHash = _hashService.Generar(datos.Password),
            Rol = datos.Rol,
            Correo = datos.Correo,
            Activo = true
        };

        await _unitOfWork.Usuarios.Agregar(usuario);
        await _unitOfWork.GuardarCambios();

        return usuario;
    }

    public async Task<Usuario> Actualizar(int id, RegistroModel datos)
    {
        Usuario usuario = await ObtenerPorId(id);

        Validar(datos, false);

        Usuario? repetido = await _unitOfWork.Usuarios.ObtenerPorUsername(datos.Username);

        if (repetido != null && repetido.UsuarioId != id)
        {
            throw new ReglaNegocioException($"Ya existe un usuario con el username {datos.Username}");
        }

        usuario.Username = datos.Username;
        usuario.Rol = datos.Rol;
        usuario.Correo = datos.Correo;

        if (!string.IsNullOrWhiteSpace(datos.Password))
        {
            usuario.PasswordHash = _hashService.Generar(datos.Password);
        }

        _unitOfWork.Usuarios.Actualizar(usuario);
        await _unitOfWork.GuardarCambios();

        return usuario;
    }

    public async Task Eliminar(int id)
    {
        Usuario usuario = await ObtenerPorId(id);

        List<Usuario> usuarios = await _unitOfWork.Usuarios.ObtenerTodos();

        if (usuario.Rol == "Admin" && usuarios.Count(u => u.Rol == "Admin") == 1)
        {
            throw new ReglaNegocioException("No se puede eliminar el unico usuario administrador del sistema");
        }

        _unitOfWork.Usuarios.Eliminar(usuario);
        await _unitOfWork.GuardarCambios();
    }

    private void Validar(RegistroModel datos, bool exigeContrasena)
    {
        if (string.IsNullOrWhiteSpace(datos.Username) || datos.Username.Length < 4)
        {
            throw new ReglaNegocioException("El username es obligatorio y debe tener al menos 4 caracteres");
        }

        if (!RolesValidos.Contains(datos.Rol))
        {
            throw new ReglaNegocioException($"El rol debe ser uno de: {string.Join(", ", RolesValidos)}");
        }

        if (datos.Correo != null && !datos.Correo.Contains('@'))
        {
            throw new ReglaNegocioException("El correo no tiene un formato valido");
        }

        if (exigeContrasena && string.IsNullOrWhiteSpace(datos.Password))
        {
            throw new ReglaNegocioException("La contrasena es obligatoria");
        }

        if (!string.IsNullOrWhiteSpace(datos.Password) && datos.Password.Length < 8)
        {
            throw new ReglaNegocioException("La contrasena debe tener al menos 8 caracteres");
        }
    }
}
