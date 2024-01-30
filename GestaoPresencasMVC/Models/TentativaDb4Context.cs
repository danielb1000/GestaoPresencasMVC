using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GestaoPresencasMVC.Models;

public partial class TentativaDb4Context : DbContext
{
    public TentativaDb4Context()
    {
    }

    public TentativaDb4Context(DbContextOptions<TentativaDb4Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Aluno> Alunos { get; set; }

    public virtual DbSet<AlunoUc> AlunoUcs { get; set; }

    public virtual DbSet<Ano> Anos { get; set; }

    public virtual DbSet<Aula> Aulas { get; set; }

    public virtual DbSet<Curso> Cursos { get; set; }

    public virtual DbSet<Docente> Docentes { get; set; }

    public virtual DbSet<Escola> Escolas { get; set; }

    public virtual DbSet<Presenca> Presencas { get; set; }

    public virtual DbSet<Uc> Ucs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=tentativa_db_4;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aluno>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Aluno__3213E83F5EDAA16B");

            entity.ToTable("Aluno");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<AlunoUc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Aluno_UC__3213E83F00EB0ADE");

            entity.ToTable("Aluno_UC");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdAluno).HasColumnName("id_aluno");
            entity.Property(e => e.IdUc).HasColumnName("id_uc");

            entity.HasOne(d => d.IdAlunoNavigation).WithMany(p => p.AlunoUcs)
                .HasForeignKey(d => d.IdAluno)
                .HasConstraintName("FK__Aluno_UC__id_alu__31EC6D26");

            entity.HasOne(d => d.IdUcNavigation).WithMany(p => p.AlunoUcs)
                .HasForeignKey(d => d.IdUc)
                .HasConstraintName("FK__Aluno_UC__id_uc__30F848ED");
        });

        modelBuilder.Entity<Ano>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ano__3213E83F38175D01");

            entity.ToTable("Ano");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Numero).HasColumnName("numero");
        });

        modelBuilder.Entity<Aula>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Aula__3213E83FD2121AB9");

            entity.ToTable("Aula");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.IdAno).HasColumnName("id_ano");
            entity.Property(e => e.IdUc).HasColumnName("id_uc");
            entity.Property(e => e.Sala)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sala");

            entity.HasOne(d => d.IdAnoNavigation).WithMany(p => p.Aulas)
                .HasForeignKey(d => d.IdAno)
                .HasConstraintName("FK__Aula__id_ano__37A5467C");

            entity.HasOne(d => d.IdUcNavigation).WithMany(p => p.Aulas)
                .HasForeignKey(d => d.IdUc)
                .HasConstraintName("FK__Aula__id_uc__36B12243");

            entity.HasMany(a => a.Presencas)
                .WithOne(p => p.IdAulaNavigation)
                .HasForeignKey(p => p.IdAula)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Curso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Curso__3213E83F08E4953F");

            entity.ToTable("Curso");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdEscola).HasColumnName("id_escola");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nome");

            entity.HasOne(d => d.IdEscolaNavigation).WithMany(p => p.Cursos)
                .HasForeignKey(d => d.IdEscola)
                .HasConstraintName("FK__Curso__id_escola__267ABA7A");
        });

        modelBuilder.Entity<Docente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Docente__3213E83F5492CD38");

            entity.ToTable("Docente");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<Escola>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Escola__3213E83F0CB413F5");

            entity.ToTable("Escola");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<Presenca>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Presenca__3213E83F8A68579C");

            entity.ToTable("Presenca");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdAluno).HasColumnName("id_aluno");
            entity.Property(e => e.IdAula).HasColumnName("id_aula");
            entity.Property(e => e.Presente).HasColumnName("presente");

            entity.HasOne(d => d.IdAlunoNavigation).WithMany(p => p.Presencas)
                .HasForeignKey(d => d.IdAluno)
                .HasConstraintName("FK__Presenca__id_alu__3B75D760");

            entity.HasOne(d => d.IdAulaNavigation).WithMany(p => p.Presencas)
                .HasForeignKey(d => d.IdAula)
                .HasConstraintName("FK__Presenca__id_aul__3A81B327");
        });

        modelBuilder.Entity<Uc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UC__3213E83F95265671");

            entity.ToTable("UC");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdCurso).HasColumnName("id_curso");
            entity.Property(e => e.IdDocente).HasColumnName("id_docente");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nome");

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.Ucs)
                .HasForeignKey(d => d.IdCurso)
                .HasConstraintName("FK__UC__id_curso__2C3393D0");

            entity.HasOne(d => d.IdDocenteNavigation).WithMany(p => p.Ucs)
                .HasForeignKey(d => d.IdDocente)
                .HasConstraintName("FK__UC__id_docente__2B3F6F97");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
