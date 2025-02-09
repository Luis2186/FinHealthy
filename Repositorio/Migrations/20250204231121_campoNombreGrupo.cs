using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class campoNombreGrupo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategorias_GruposDeGasto_FamiliaId",
                schema: "identity",
                table: "SubCategorias");

            migrationBuilder.RenameColumn(
                name: "FamiliaId",
                schema: "identity",
                table: "SubCategorias",
                newName: "GrupoGastoId");

            migrationBuilder.RenameIndex(
                name: "IX_SubCategorias_FamiliaId",
                schema: "identity",
                table: "SubCategorias",
                newName: "IX_SubCategorias_GrupoGastoId");

            migrationBuilder.RenameColumn(
                name: "Apellido",
                schema: "identity",
                table: "GruposDeGasto",
                newName: "Nombre");

            migrationBuilder.RenameIndex(
                name: "IX_GruposDeGasto_Apellido",
                schema: "identity",
                table: "GruposDeGasto",
                newName: "IX_GruposDeGasto_Nombre");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategorias_GruposDeGasto_GrupoGastoId",
                schema: "identity",
                table: "SubCategorias");

            migrationBuilder.RenameColumn(
                name: "GrupoGastoId",
                schema: "identity",
                table: "SubCategorias",
                newName: "FamiliaId");

            migrationBuilder.RenameIndex(
                name: "IX_SubCategorias_GrupoGastoId",
                schema: "identity",
                table: "SubCategorias",
                newName: "IX_SubCategorias_FamiliaId");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                schema: "identity",
                table: "GruposDeGasto",
                newName: "Apellido");

            migrationBuilder.RenameIndex(
                name: "IX_GruposDeGasto_Nombre",
                schema: "identity",
                table: "GruposDeGasto",
                newName: "IX_GruposDeGasto_Apellido");

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
    }
}
