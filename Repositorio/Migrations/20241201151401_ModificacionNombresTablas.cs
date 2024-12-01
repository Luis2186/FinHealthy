using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class ModificacionNombresTablas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MiembrosFamiliares_GruposFamiliares_GrupoFamiliarId",
                schema: "identity",
                table: "MiembrosFamiliares");

            migrationBuilder.DropForeignKey(
                name: "FK_SolcitudesGrupoFamiliar_AspNetUsers_UsuarioAdministradorGrupoId",
                schema: "identity",
                table: "SolcitudesGrupoFamiliar");

            migrationBuilder.DropForeignKey(
                name: "FK_SolcitudesGrupoFamiliar_AspNetUsers_UsuarioSolicitanteId",
                schema: "identity",
                table: "SolcitudesGrupoFamiliar");

            migrationBuilder.DropTable(
                name: "GruposFamiliares",
                schema: "identity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SolcitudesGrupoFamiliar",
                schema: "identity",
                table: "SolcitudesGrupoFamiliar");

            migrationBuilder.RenameTable(
                name: "SolcitudesGrupoFamiliar",
                schema: "identity",
                newName: "SolcitudesUnionFamilia",
                newSchema: "identity");

            migrationBuilder.RenameIndex(
                name: "IX_SolcitudesGrupoFamiliar_UsuarioSolicitanteId",
                schema: "identity",
                table: "SolcitudesUnionFamilia",
                newName: "IX_SolcitudesUnionFamilia_UsuarioSolicitanteId");

            migrationBuilder.RenameIndex(
                name: "IX_SolcitudesGrupoFamiliar_UsuarioAdministradorGrupoId",
                schema: "identity",
                table: "SolcitudesUnionFamilia",
                newName: "IX_SolcitudesUnionFamilia_UsuarioAdministradorGrupoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolcitudesUnionFamilia",
                schema: "identity",
                table: "SolcitudesUnionFamilia",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Familias",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UsuarioAdministradorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CodigoAccesoHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Familias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Familias_AspNetUsers_UsuarioAdministradorId",
                        column: x => x.UsuarioAdministradorId,
                        principalSchema: "identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Familias_UsuarioAdministradorId",
                schema: "identity",
                table: "Familias",
                column: "UsuarioAdministradorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MiembrosFamiliares_Familias_GrupoFamiliarId",
                schema: "identity",
                table: "MiembrosFamiliares",
                column: "GrupoFamiliarId",
                principalSchema: "identity",
                principalTable: "Familias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SolcitudesUnionFamilia_AspNetUsers_UsuarioAdministradorGrupoId",
                schema: "identity",
                table: "SolcitudesUnionFamilia",
                column: "UsuarioAdministradorGrupoId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SolcitudesUnionFamilia_AspNetUsers_UsuarioSolicitanteId",
                schema: "identity",
                table: "SolcitudesUnionFamilia",
                column: "UsuarioSolicitanteId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MiembrosFamiliares_Familias_GrupoFamiliarId",
                schema: "identity",
                table: "MiembrosFamiliares");

            migrationBuilder.DropForeignKey(
                name: "FK_SolcitudesUnionFamilia_AspNetUsers_UsuarioAdministradorGrupoId",
                schema: "identity",
                table: "SolcitudesUnionFamilia");

            migrationBuilder.DropForeignKey(
                name: "FK_SolcitudesUnionFamilia_AspNetUsers_UsuarioSolicitanteId",
                schema: "identity",
                table: "SolcitudesUnionFamilia");

            migrationBuilder.DropTable(
                name: "Familias",
                schema: "identity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SolcitudesUnionFamilia",
                schema: "identity",
                table: "SolcitudesUnionFamilia");

            migrationBuilder.RenameTable(
                name: "SolcitudesUnionFamilia",
                schema: "identity",
                newName: "SolcitudesGrupoFamiliar",
                newSchema: "identity");

            migrationBuilder.RenameIndex(
                name: "IX_SolcitudesUnionFamilia_UsuarioSolicitanteId",
                schema: "identity",
                table: "SolcitudesGrupoFamiliar",
                newName: "IX_SolcitudesGrupoFamiliar_UsuarioSolicitanteId");

            migrationBuilder.RenameIndex(
                name: "IX_SolcitudesUnionFamilia_UsuarioAdministradorGrupoId",
                schema: "identity",
                table: "SolcitudesGrupoFamiliar",
                newName: "IX_SolcitudesGrupoFamiliar_UsuarioAdministradorGrupoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolcitudesGrupoFamiliar",
                schema: "identity",
                table: "SolcitudesGrupoFamiliar",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "GruposFamiliares",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioAdministradorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CodigoAccesoHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    FechaDeCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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

            migrationBuilder.CreateIndex(
                name: "IX_GruposFamiliares_UsuarioAdministradorId",
                schema: "identity",
                table: "GruposFamiliares",
                column: "UsuarioAdministradorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MiembrosFamiliares_GruposFamiliares_GrupoFamiliarId",
                schema: "identity",
                table: "MiembrosFamiliares",
                column: "GrupoFamiliarId",
                principalSchema: "identity",
                principalTable: "GruposFamiliares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SolcitudesGrupoFamiliar_AspNetUsers_UsuarioAdministradorGrupoId",
                schema: "identity",
                table: "SolcitudesGrupoFamiliar",
                column: "UsuarioAdministradorGrupoId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SolcitudesGrupoFamiliar_AspNetUsers_UsuarioSolicitanteId",
                schema: "identity",
                table: "SolcitudesGrupoFamiliar",
                column: "UsuarioSolicitanteId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
