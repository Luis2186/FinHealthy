using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class Subcategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategorias_GruposDeGasto_GrupoGastoId",
                schema: "identity",
                table: "SubCategorias");

            migrationBuilder.RenameColumn(
                name: "GrupoGastoId",
                schema: "identity",
                table: "SubCategorias",
                newName: "GrupoId");

            migrationBuilder.RenameIndex(
                name: "IX_SubCategorias_GrupoGastoId",
                schema: "identity",
                table: "SubCategorias",
                newName: "IX_SubCategorias_GrupoId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategorias_GruposDeGasto_GrupoId",
                schema: "identity",
                table: "SubCategorias",
                column: "GrupoId",
                principalSchema: "identity",
                principalTable: "GruposDeGasto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategorias_GruposDeGasto_GrupoId",
                schema: "identity",
                table: "SubCategorias");

            migrationBuilder.RenameColumn(
                name: "GrupoId",
                schema: "identity",
                table: "SubCategorias",
                newName: "GrupoGastoId");

            migrationBuilder.RenameIndex(
                name: "IX_SubCategorias_GrupoId",
                schema: "identity",
                table: "SubCategorias",
                newName: "IX_SubCategorias_GrupoGastoId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategorias_GruposDeGasto_GrupoGastoId",
                schema: "identity",
                table: "SubCategorias",
                column: "GrupoGastoId",
                principalSchema: "identity",
                principalTable: "GruposDeGasto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
