using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class SincronizarModeloGasto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GrupoId",
                table: "Gastos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioCreadorId",
                table: "Gastos",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_GrupoId",
                table: "Gastos",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_UsuarioCreadorId",
                table: "Gastos",
                column: "UsuarioCreadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_AspNetUsers_UsuarioCreadorId",
                table: "Gastos",
                column: "UsuarioCreadorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_GruposDeGasto_GrupoId",
                table: "Gastos",
                column: "GrupoId",
                principalTable: "GruposDeGasto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_AspNetUsers_UsuarioCreadorId",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_GruposDeGasto_GrupoId",
                table: "Gastos");

            migrationBuilder.DropIndex(
                name: "IX_Gastos_GrupoId",
                table: "Gastos");

            migrationBuilder.DropIndex(
                name: "IX_Gastos_UsuarioCreadorId",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "GrupoId",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "UsuarioCreadorId",
                table: "Gastos");
        }
    }
}
