using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SistemaGestionRiesgos.Models;

namespace SistemaGestionRiesgos.Context;

public partial class GestionDbContext : DbContext
{
    public GestionDbContext()
    {
    }

    public GestionDbContext(DbContextOptions<GestionDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Plan> Planes { get; set; }

    public virtual DbSet<Riesgo> Riesgos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("workstation id=ingenieria2024.mssql.somee.com;packet size=4096;user id=ingenieria2024_SQLLogin_1;pwd=llx8ynvybj;data source=ingenieria2024.mssql.somee.com;persist security info=False;initial catalog=ingenieria2024;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Plan>(entity =>
        {
            entity.HasKey(e => e.IdPlan).HasName("PK__Planes__FB8102AEE1D18B49");

            entity.HasIndex(e => e.IdRiesgo, "IX_Planes_IdRiesgo");

            entity.HasIndex(e => e.IdUsuario, "IX_Planes_IdUsuarioNavigationIdUsuario");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRiesgoNavigation).WithMany(p => p.Planes)
                .HasForeignKey(d => d.IdRiesgo)
                .HasConstraintName("FK__Planes__IdRiesgo__32E0915F");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Planes)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_Planes_Usuarios_IdUsuarioNavigationIdUsuario");
        });

        modelBuilder.Entity<Riesgo>(entity =>
        {
            entity.HasKey(e => e.IdRiesgo).HasName("PK__Riesgos__5D672788F5B82265");

            entity.HasIndex(e => e.IdUsuario, "IX_Riesgos_IdUsuarioNavigationIdUsuario");

            entity.Property(e => e.Causa)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Consecuencia)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Impacto)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Probabilidad)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Riesgos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_Riesgos_Usuarios_IdUsuarioNavigationIdUsuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
