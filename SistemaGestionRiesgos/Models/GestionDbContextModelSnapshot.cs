﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SistemaGestionRiesgos.Context;

#nullable disable

namespace SistemaGestionRiesgos.Models
{
    [DbContext(typeof(GestionDbContext))]
    partial class GestionDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SistemaGestionRiesgos.Models.Bitacora", b =>
                {
                    b.Property<int>("IdBitacora")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdBitacora"));

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int?>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<int?>("IdUsuarioNavigationIdUsuario")
                        .HasColumnType("int");

                    b.Property<string>("Tabla")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TipoAccion")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdBitacora");

                    b.HasIndex("IdUsuarioNavigationIdUsuario");

                    b.ToTable("Bitacoras");
                });

            modelBuilder.Entity("SistemaGestionRiesgos.Models.Plan", b =>
                {
                    b.Property<int>("IdPlan")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPlan"));

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IdRiesgo")
                        .HasColumnType("int");

                    b.Property<int?>("IdRiesgoNavigationIdRiesgo")
                        .HasColumnType("int");

                    b.Property<int?>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<int?>("IdUsuarioNavigationIdUsuario")
                        .HasColumnType("int");

                    b.Property<string>("TipoPlan")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdPlan");

                    b.HasIndex("IdRiesgoNavigationIdRiesgo");

                    b.HasIndex("IdUsuarioNavigationIdUsuario");

                    b.ToTable("Planes");
                });

            modelBuilder.Entity("SistemaGestionRiesgos.Models.Riesgo", b =>
                {
                    b.Property<int>("IdRiesgo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRiesgo"));

                    b.Property<string>("Causa")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Consecuencia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<int?>("IdUsuarioNavigationIdUsuario")
                        .HasColumnType("int");

                    b.Property<string>("Impacto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Probabilidad")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Titulo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdRiesgo");

                    b.HasIndex("IdUsuarioNavigationIdUsuario");

                    b.ToTable("Riesgos");
                });

            modelBuilder.Entity("SistemaGestionRiesgos.Models.Usuario", b =>
                {
                    b.Property<int>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUsuario"));

                    b.Property<bool?>("Activo")
                        .HasColumnType("bit");

                    b.Property<string>("Contraseña")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContraseñaTemporal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("PrimerInicio")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdUsuario");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("SistemaGestionRiesgos.Models.Bitacora", b =>
                {
                    b.HasOne("SistemaGestionRiesgos.Models.Usuario", "IdUsuarioNavigation")
                        .WithMany("Bitacoras")
                        .HasForeignKey("IdUsuarioNavigationIdUsuario");

                    b.Navigation("IdUsuarioNavigation");
                });

            modelBuilder.Entity("SistemaGestionRiesgos.Models.Plan", b =>
                {
                    b.HasOne("SistemaGestionRiesgos.Models.Riesgo", "IdRiesgoNavigation")
                        .WithMany("Planes")
                        .HasForeignKey("IdRiesgoNavigationIdRiesgo");

                    b.HasOne("SistemaGestionRiesgos.Models.Usuario", "IdUsuarioNavigation")
                        .WithMany("Planes")
                        .HasForeignKey("IdUsuarioNavigationIdUsuario");

                    b.Navigation("IdRiesgoNavigation");

                    b.Navigation("IdUsuarioNavigation");
                });

            modelBuilder.Entity("SistemaGestionRiesgos.Models.Riesgo", b =>
                {
                    b.HasOne("SistemaGestionRiesgos.Models.Usuario", "IdUsuarioNavigation")
                        .WithMany("Riesgos")
                        .HasForeignKey("IdUsuarioNavigationIdUsuario");

                    b.Navigation("IdUsuarioNavigation");
                });

            modelBuilder.Entity("SistemaGestionRiesgos.Models.Riesgo", b =>
                {
                    b.Navigation("Planes");
                });

            modelBuilder.Entity("SistemaGestionRiesgos.Models.Usuario", b =>
                {
                    b.Navigation("Bitacoras");

                    b.Navigation("Planes");

                    b.Navigation("Riesgos");
                });
#pragma warning restore 612, 618
        }
    }
}