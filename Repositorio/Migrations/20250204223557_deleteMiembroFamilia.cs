using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class deleteMiembroFamilia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategorias_Familias_FamiliaId",
                schema: "identity",
                table: "SubCategorias");

            migrationBuilder.DropTable(
                name: "MiembrosFamiliares",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "Familias",
                schema: "identity");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDeUnion",
                schema: "identity",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GrupoDeGastosId",
                schema: "identity",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GruposDeGasto",
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
                    table.PrimaryKey("PK_GruposDeGasto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GruposDeGasto_AspNetUsers_UsuarioAdministradorId",
                        column: x => x.UsuarioAdministradorId,
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
                name: "IX_GruposDeGasto_Apellido",
                schema: "identity",
                table: "GruposDeGasto",
                column: "Apellido",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GruposDeGasto_UsuarioAdministradorId",
                schema: "identity",
                table: "GruposDeGasto",
                column: "UsuarioAdministradorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_GruposDeGasto_GrupoDeGastosId",
                schema: "identity",
                table: "AspNetUsers",
                column: "GrupoDeGastosId",
                principalSchema: "identity",
                principalTable: "GruposDeGasto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategorias_GruposDeGasto_FamiliaId",
                schema: "identity",
                table: "SubCategorias",
                column: "FamiliaId",
                principalSchema: "identity",
                principalTable: "GruposDeGasto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_GruposDeGasto_GrupoDeGastosId",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategorias_GruposDeGasto_FamiliaId",
                schema: "identity",
                table: "SubCategorias");

            migrationBuilder.DropTable(
                name: "GruposDeGasto",
                schema: "identity");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GrupoDeGastosId",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FechaDeUnion",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GrupoDeGastosId",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Familias",
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
                    table.PrimaryKey("PK_Familias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Familias_AspNetUsers_UsuarioAdministradorId",
                        column: x => x.UsuarioAdministradorId,
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
                    GrupoFamiliarId = table.Column<int>(type: "int", nullable: true),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaDeUnion = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                        name: "FK_MiembrosFamiliares_Familias_GrupoFamiliarId",
                        column: x => x.GrupoFamiliarId,
                        principalSchema: "identity",
                        principalTable: "Familias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Familias_Apellido",
                schema: "identity",
                table: "Familias",
                column: "Apellido",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Familias_UsuarioAdministradorId",
                schema: "identity",
                table: "Familias",
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

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategorias_Familias_FamiliaId",
                schema: "identity",
                table: "SubCategorias",
                column: "FamiliaId",
                principalSchema: "identity",
                principalTable: "Familias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
