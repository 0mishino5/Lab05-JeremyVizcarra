namespace Lab05_JeremyVizcarra.Domain.Entities;

public partial class Estudiante
{
    public int EstudianteId { get; set; }

    public string Nombre { get; set; } = null!;

    public int Edad { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();

    public virtual ICollection<Evaluacion> Evaluaciones { get; set; } = new List<Evaluacion>();

    public virtual ICollection<Asistencia> Asistencias { get; set; } = new List<Asistencia>();
}
