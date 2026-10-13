namespace Lab05_JeremyVizcarra.Application.Models;

public class RegistroModel
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public string? Correo { get; set; }
}
