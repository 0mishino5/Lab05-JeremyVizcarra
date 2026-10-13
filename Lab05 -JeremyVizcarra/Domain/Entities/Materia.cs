namespace Lab05_JeremyVizcarra.Domain.Entities;

public partial class Materia
{
    public int MateriaId { get; set; }

    public int? CursoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual Curso? Curso { get; set; }
}
