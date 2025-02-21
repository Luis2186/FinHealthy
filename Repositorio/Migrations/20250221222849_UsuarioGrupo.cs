using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class UsuarioGrupo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_GruposDeGasto_GrupoDeGastosId",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SolcitudesUnionFamilia",
                schema: "identity");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GrupoDeGastosId",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "SolcitudesUnionGrupo",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioAdministradorGrupoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CodigoDeSeguridad = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    EstadoSolicitudGrupo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioSolicitanteId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FechaDeEnvio = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaDeRespuesta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MensajeOpcional = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolcitudesUnionGrupo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolcitudesUnionGrupo_AspNetUsers_UsuarioAdministradorGrupoId",
                        column: x => x.UsuarioAdministradorGrupoId,
                        principalSchema: "identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolcitudesUnionGrupo_AspNetUsers_UsuarioSolicitanteId",
                        column: x => x.UsuarioSolicitanteId,
                        principalSchema: "identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioGrupos",
                schema: "identity",
                columns: table => new
                {
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GrupoId = table.Column<int>(type: "int", nullable: false),
                    FechaDeUnion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioGrupos", x => new { x.GrupoId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_UsuarioGrupos_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioGrupos_GruposDeGasto_GrupoId",
                        column: x => x.GrupoId,
                        principalSchema: "identity",
                        principalTable: "GruposDeGasto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolcitudesUnionGrupo_UsuarioAdministradorGrupoId",
                schema: "identity",
                table: "SolcitudesUnionGrupo",
                column: "UsuarioAdministradorGrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_SolcitudesUnionGrupo_UsuarioSolicitanteId",
                schema: "identity",
                table: "SolcitudesUnionGrupo",
                column: "UsuarioSolicitanteId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioGrupos_UsuarioId",
                schema: "identity",
                table: "UsuarioGrupos",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolcitudesUnionGrupo",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "UsuarioGrupos",
                schema: "identity");

            migrationBuilder.CreateTable(
                name: "SolcitudesUnionFamilia",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioAdministradorGrupoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsuarioSolicitanteId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Activa = table.Column<bool>(type: "bit", nullable: false),
                    CodigoDeSeguridad = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EstadoSolicitudGrupo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaDeEnvio = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    FechaDeRespuesta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MensajeOpcional = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolcitudesUnionFamilia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolcitudesUnionFamilia_AspNetUsers_UsuarioAdministradorGrupoId",
                        column: x => x.UsuarioAdministradorGrupoId,
                        principalSchema: "identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolcitudesUnionFamilia_AspNetUsers_UsuarioSolicitanteId",
                        column: x => x.UsuarioSolicitanteId,
                        principalSchema: "identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GrupoDeGastosId",
                schema: "identity",
                table: "AspNetUsers",
                column: "GrupoDeGastosId");

            migrationBuilder.CreateIndex(
                name: "IX_SolcitudesUnionFamilia_UsuarioAdministradorGrupoId",
                schema: "identity",
                table: "SolcitudesUnionFamilia",
                column: "UsuarioAdministradorGrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_SolcitudesUnionFamilia_UsuarioSolicitanteId",
                schema: "identity",
                table: "SolcitudesUnionFamilia",
                column: "UsuarioSolicitanteId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_GruposDeGasto_GrupoDeGastosId",
                schema: "identity",
                table: "AspNetUsers",
                column: "GrupoDeGastosId",
                principalSchema: "identity",
                principalTable: "GruposDeGasto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
