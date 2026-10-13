namespace Lab05_JeremyVizcarra.Domain.Entities;

public partial class Matricula
{
    public int MatriculaId { get; set; }

    public int? EstudianteId { get; set; }

    public int? CursoId { get; set; }

    public string? Semestre { get; set; }

    public virtual Estudiante? Estudiante { get; set; }

    public virtual Curso? Curso { get; set; }
}
