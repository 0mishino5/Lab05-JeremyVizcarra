namespace Lab05_JeremyVizcarra.Domain.Entities;

public partial class Curso
{
    public int CursoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int Creditos { get; set; }

    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();

    public virtual ICollection<Materia> Materias { get; set; } = new List<Materia>();

    public virtual ICollection<Evaluacion> Evaluaciones { get; set; } = new List<Evaluacion>();

    public virtual ICollection<Asistencia> Asistencias { get; set; } = new List<Asistencia>();
}
