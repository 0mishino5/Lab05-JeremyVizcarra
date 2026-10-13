namespace Lab05_JeremyVizcarra.Domain.Entities;

public partial class Evaluacion
{
    public int EvaluacionId { get; set; }

    public int? EstudianteId { get; set; }

    public int? CursoId { get; set; }

    public decimal? Calificacion { get; set; }

    public DateOnly? Fecha { get; set; }

    public virtual Estudiante? Estudiante { get; set; }

    public virtual Curso? Curso { get; set; }
}
