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
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Plan>(entity =>
        {
            entity.HasKey(e => e.IdPlan).HasName("PK__Planes__FB8102AEE1D18B49");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRiesgoNavigation).WithMany(p => p.Planes)
                .HasForeignKey(d => d.IdRiesgo)
                .HasConstraintName("FK__Planes__IdRiesgo__32E0915F");
        });

        modelBuilder.Entity<Riesgo>(entity =>
        {
            entity.HasKey(e => e.IdRiesgo).HasName("PK__Riesgos__5D672788F5B82265");

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
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
