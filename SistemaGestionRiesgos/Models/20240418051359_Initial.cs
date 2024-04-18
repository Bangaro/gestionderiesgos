using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaGestionRiesgos.Models
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContraseñaTemporal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contraseña = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: true),
                    PrimerInicio = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Bitacoras",
                columns: table => new
                {
                    IdBitacora = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoAccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tabla = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUsuario = table.Column<int>(type: "int", nullable: true),
                    IdUsuarioNavigationIdUsuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bitacoras", x => x.IdBitacora);
                    table.ForeignKey(
                        name: "FK_Bitacoras_Usuarios_IdUsuarioNavigationIdUsuario",
                        column: x => x.IdUsuarioNavigationIdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Riesgos",
                columns: table => new
                {
                    IdRiesgo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Impacto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Probabilidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Causa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Consecuencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUsuario = table.Column<int>(type: "int", nullable: true),
                    IdUsuarioNavigationIdUsuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Riesgos", x => x.IdRiesgo);
                    table.ForeignKey(
                        name: "FK_Riesgos_Usuarios_IdUsuarioNavigationIdUsuario",
                        column: x => x.IdUsuarioNavigationIdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Planes",
                columns: table => new
                {
                    IdPlan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoPlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdRiesgo = table.Column<int>(type: "int", nullable: true),
                    IdUsuario = table.Column<int>(type: "int", nullable: true),
                    IdRiesgoNavigationIdRiesgo = table.Column<int>(type: "int", nullable: true),
                    IdUsuarioNavigationIdUsuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planes", x => x.IdPlan);
                    table.ForeignKey(
                        name: "FK_Planes_Riesgos_IdRiesgoNavigationIdRiesgo",
                        column: x => x.IdRiesgoNavigationIdRiesgo,
                        principalTable: "Riesgos",
                        principalColumn: "IdRiesgo");
                    table.ForeignKey(
                        name: "FK_Planes_Usuarios_IdUsuarioNavigationIdUsuario",
                        column: x => x.IdUsuarioNavigationIdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bitacoras_IdUsuarioNavigationIdUsuario",
                table: "Bitacoras",
                column: "IdUsuarioNavigationIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Planes_IdRiesgoNavigationIdRiesgo",
                table: "Planes",
                column: "IdRiesgoNavigationIdRiesgo");

            migrationBuilder.CreateIndex(
                name: "IX_Planes_IdUsuarioNavigationIdUsuario",
                table: "Planes",
                column: "IdUsuarioNavigationIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Riesgos_IdUsuarioNavigationIdUsuario",
                table: "Riesgos",
                column: "IdUsuarioNavigationIdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bitacoras");

            migrationBuilder.DropTable(
                name: "Planes");

            migrationBuilder.DropTable(
                name: "Riesgos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
