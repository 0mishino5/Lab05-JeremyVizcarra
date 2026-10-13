namespace Lab05_JeremyVizcarra.Domain.Entities;

public partial class Asistencia
{
    public int AsistenciaId { get; set; }

    public int? EstudianteId { get; set; }

    public int? CursoId { get; set; }

    public DateOnly? Fecha { get; set; }

    public string? Estado { get; set; }

    public virtual Estudiante? Estudiante { get; set; }

    public virtual Curso? Curso { get; set; }
}
