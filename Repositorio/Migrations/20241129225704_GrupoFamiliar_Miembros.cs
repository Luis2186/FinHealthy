using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class GrupoFamiliar_Miembros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GruposFamiliares",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UsuarioAdministradorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GruposFamiliares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GruposFamiliares_AspNetUsers_UsuarioAdministradorId",
                        column: x => x.UsuarioAdministradorId,
                        principalSchema: "identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SolcitudesGrupoFamiliar",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioSolicitanteId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsuarioAdministradorGrupoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CodigoDeSeguridad = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FechaDeEnvio = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaDeRespuesta = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    MensajeOpcional = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoSolicitudGrupo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolcitudesGrupoFamiliar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolcitudesGrupoFamiliar_AspNetUsers_UsuarioAdministradorGrupoId",
                        column: x => x.UsuarioAdministradorGrupoId,
                        principalSchema: "identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolcitudesGrupoFamiliar_AspNetUsers_UsuarioSolicitanteId",
                        column: x => x.UsuarioSolicitanteId,
                        principalSchema: "identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MiembrosFamiliares",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FechaDeUnion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    GrupoFamiliarId = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiembrosFamiliares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MiembrosFamiliares_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MiembrosFamiliares_GruposFamiliares_GrupoFamiliarId",
                        column: x => x.GrupoFamiliarId,
                        principalSchema: "identity",
                        principalTable: "GruposFamiliares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GruposFamiliares_UsuarioAdministradorId",
                schema: "identity",
                table: "GruposFamiliares",
                column: "UsuarioAdministradorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MiembrosFamiliares_GrupoFamiliarId",
                schema: "identity",
                table: "MiembrosFamiliares",
                column: "GrupoFamiliarId");

            migrationBuilder.CreateIndex(
                name: "IX_MiembrosFamiliares_UsuarioId",
                schema: "identity",
                table: "MiembrosFamiliares",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_SolcitudesGrupoFamiliar_UsuarioAdministradorGrupoId",
                schema: "identity",
                table: "SolcitudesGrupoFamiliar",
                column: "UsuarioAdministradorGrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_SolcitudesGrupoFamiliar_UsuarioSolicitanteId",
                schema: "identity",
                table: "SolcitudesGrupoFamiliar",
                column: "UsuarioSolicitanteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MiembrosFamiliares",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "SolcitudesGrupoFamiliar",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "GruposFamiliares",
                schema: "identity");
        }
    }
}
