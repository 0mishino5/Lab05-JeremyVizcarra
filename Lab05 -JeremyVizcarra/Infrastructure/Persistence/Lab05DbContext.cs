using Lab05_JeremyVizcarra.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lab05_JeremyVizcarra.Infrastructure.Persistence;

public partial class Lab05DbContext : DbContext
{
    public Lab05DbContext(DbContextOptions<Lab05DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<Curso> Cursos { get; set; }

    public virtual DbSet<Profesor> Profesores { get; set; }

    public virtual DbSet<Matricula> Matriculas { get; set; }

    public virtual DbSet<Evaluacion> Evaluaciones { get; set; }

    public virtual DbSet<Asistencia> Asistencias { get; set; }

    public virtual DbSet<Materia> Materias { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.HasKey(e => e.EstudianteId).HasName("estudiantes_pkey");

            entity.ToTable("estudiantes");

            entity.Property(e => e.EstudianteId).HasColumnName("id_estudiante");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Edad).HasColumnName("edad");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .HasColumnName("direccion");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .HasColumnName("correo");
        });

        modelBuilder.Entity<Curso>(entity =>
        {
            entity.HasKey(e => e.CursoId).HasName("cursos_pkey");

            entity.ToTable("cursos");

            entity.Property(e => e.CursoId).HasColumnName("id_curso");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Creditos).HasColumnName("creditos");
        });

        modelBuilder.Entity<Profesor>(entity =>
        {
            entity.HasKey(e => e.ProfesorId).HasName("profesores_pkey");

            entity.ToTable("profesores");

            entity.Property(e => e.ProfesorId).HasColumnName("id_profesor");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Especialidad)
                .HasMaxLength(100)
                .HasColumnName("especialidad");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .HasColumnName("correo");
        });

        modelBuilder.Entity<Matricula>(entity =>
        {
            entity.HasKey(e => e.MatriculaId).HasName("matriculas_pkey");

            entity.ToTable("matriculas");

            entity.Property(e => e.MatriculaId).HasColumnName("id_matricula");
            entity.Property(e => e.EstudianteId).HasColumnName("id_estudiante");
            entity.Property(e => e.CursoId).HasColumnName("id_curso");
            entity.Property(e => e.Semestre)
                .HasMaxLength(20)
                .HasColumnName("semestre");

            entity.HasOne(d => d.Estudiante).WithMany(p => p.Matriculas)
                .HasForeignKey(d => d.EstudianteId)
                .HasConstraintName("matriculas_id_estudiante_fkey");

            entity.HasOne(d => d.Curso).WithMany(p => p.Matriculas)
                .HasForeignKey(d => d.CursoId)
                .HasConstraintName("matriculas_id_curso_fkey");
        });

        modelBuilder.Entity<Evaluacion>(entity =>
        {
            entity.HasKey(e => e.EvaluacionId).HasName("evaluaciones_pkey");

            entity.ToTable("evaluaciones");

            entity.Property(e => e.EvaluacionId).HasColumnName("id_evaluacion");
            entity.Property(e => e.EstudianteId).HasColumnName("id_estudiante");
            entity.Property(e => e.CursoId).HasColumnName("id_curso");
            entity.Property(e => e.Calificacion)
                .HasPrecision(5, 2)
                .HasColumnName("calificacion");
            entity.Property(e => e.Fecha).HasColumnName("fecha");

            entity.HasOne(d => d.Estudiante).WithMany(p => p.Evaluaciones)
                .HasForeignKey(d => d.EstudianteId)
                .HasConstraintName("evaluaciones_id_estudiante_fkey");

            entity.HasOne(d => d.Curso).WithMany(p => p.Evaluaciones)
                .HasForeignKey(d => d.CursoId)
                .HasConstraintName("evaluaciones_id_curso_fkey");
        });

        modelBuilder.Entity<Asistencia>(entity =>
        {
            entity.HasKey(e => e.AsistenciaId).HasName("asistencias_pkey");

            entity.ToTable("asistencias");

            entity.Property(e => e.AsistenciaId).HasColumnName("id_asistencia");
            entity.Property(e => e.EstudianteId).HasColumnName("id_estudiante");
            entity.Property(e => e.CursoId).HasColumnName("id_curso");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasColumnName("estado");

            entity.HasOne(d => d.Estudiante).WithMany(p => p.Asistencias)
                .HasForeignKey(d => d.EstudianteId)
                .HasConstraintName("asistencias_id_estudiante_fkey");

            entity.HasOne(d => d.Curso).WithMany(p => p.Asistencias)
                .HasForeignKey(d => d.CursoId)
                .HasConstraintName("asistencias_id_curso_fkey");
        });

        modelBuilder.Entity<Materia>(entity =>
        {
            entity.HasKey(e => e.MateriaId).HasName("materias_pkey");

            entity.ToTable("materias");

            entity.Property(e => e.MateriaId).HasColumnName("id_materia");
            entity.Property(e => e.CursoId).HasColumnName("id_curso");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");

            entity.HasOne(d => d.Curso).WithMany(p => p.Materias)
                .HasForeignKey(d => d.CursoId)
                .HasConstraintName("materias_id_curso_fkey");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Username).IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("id_usuario");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(200)
                .HasColumnName("password_hash");
            entity.Property(e => e.Rol)
                .HasMaxLength(20)
                .HasColumnName("rol");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .HasColumnName("correo");
            entity.Property(e => e.Activo).HasColumnName("activo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
