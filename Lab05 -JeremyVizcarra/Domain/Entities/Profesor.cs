namespace Lab05_JeremyVizcarra.Domain.Entities;

public partial class Profesor
{
    public int ProfesorId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Especialidad { get; set; }

    public string? Correo { get; set; }
}
