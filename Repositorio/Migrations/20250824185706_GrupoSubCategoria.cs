using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class GrupoSubCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategorias_Categorias_CategoriaId",
                table: "SubCategorias");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategorias_GruposDeGasto_GrupoId",
                table: "SubCategorias");

            migrationBuilder.DropIndex(
                name: "IX_SubCategorias_GrupoId",
                table: "SubCategorias");

            migrationBuilder.DropIndex(
                name: "IX_SubCategorias_Nombre",
                table: "SubCategorias");

            migrationBuilder.DropColumn(
                name: "GrupoId",
                table: "SubCategorias");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "SubCategorias",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "SubCategorias",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "GrupoSubCategorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrupoId = table.Column<int>(type: "int", nullable: false),
                    SubCategoriaId = table.Column<int>(type: "int", nullable: false),
                    NombrePersonalizado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoSubCategorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrupoSubCategorias_GruposDeGasto_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "GruposDeGasto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrupoSubCategorias_SubCategorias_SubCategoriaId",
                        column: x => x.SubCategoriaId,
                        principalTable: "SubCategorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubCategorias_Nombre",
                table: "SubCategorias",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GrupoSubCategorias_GrupoId_SubCategoriaId",
                table: "GrupoSubCategorias",
                columns: new[] { "GrupoId", "SubCategoriaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GrupoSubCategorias_SubCategoriaId",
                table: "GrupoSubCategorias",
                column: "SubCategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategorias_Categorias_CategoriaId",
                table: "SubCategorias",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategorias_Categorias_CategoriaId",
                table: "SubCategorias");

            migrationBuilder.DropTable(
                name: "GrupoSubCategorias");

            migrationBuilder.DropIndex(
                name: "IX_SubCategorias_Nombre",
                table: "SubCategorias");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "SubCategorias",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "SubCategorias",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GrupoId",
                table: "SubCategorias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategorias_GrupoId",
                table: "SubCategorias",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategorias_Nombre",
                table: "SubCategorias",
                column: "Nombre",
                unique: true,
                filter: "[Nombre] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategorias_Categorias_CategoriaId",
                table: "SubCategorias",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategorias_GruposDeGasto_GrupoId",
                table: "SubCategorias",
                column: "GrupoId",
                principalTable: "GruposDeGasto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
