namespace Lab05_JeremyVizcarra.Application.Models;

public class TokenModel
{
    public string Token { get; set; } = null!;

    public DateTime Expira { get; set; }

    public string Username { get; set; } = null!;

    public string Rol { get; set; } = null!;
}
