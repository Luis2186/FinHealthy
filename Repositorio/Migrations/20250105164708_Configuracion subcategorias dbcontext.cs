using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class Configuracionsubcategoriasdbcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategorias_Categorias_CategoriaId",
                schema: "identity",
                table: "SubCategorias");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategorias_Familias_FamiliaId",
                schema: "identity",
                table: "SubCategorias");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategorias_Categorias_CategoriaId",
                schema: "identity",
                table: "SubCategorias",
                column: "CategoriaId",
                principalSchema: "identity",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategorias_Categorias_CategoriaId",
                schema: "identity",
                table: "SubCategorias");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategorias_Familias_FamiliaId",
                schema: "identity",
                table: "SubCategorias");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategorias_Categorias_CategoriaId",
                schema: "identity",
                table: "SubCategorias",
                column: "CategoriaId",
                principalSchema: "identity",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategorias_Familias_FamiliaId",
                schema: "identity",
                table: "SubCategorias",
                column: "FamiliaId",
                principalSchema: "identity",
                principalTable: "Familias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
