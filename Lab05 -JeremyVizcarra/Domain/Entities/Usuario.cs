namespace Lab05_JeremyVizcarra.Domain.Entities;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public string? Correo { get; set; }

    public bool Activo { get; set; }
}
