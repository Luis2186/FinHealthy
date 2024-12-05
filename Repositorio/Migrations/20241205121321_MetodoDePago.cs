using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class MetodoDePago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                schema: "identity",
                table: "TipoDeDocumentos",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                schema: "identity",
                table: "Monedas",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                schema: "identity",
                table: "Categorias",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "MetodosDePago",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetodosDePago", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TipoDeDocumentos_Nombre",
                schema: "identity",
                table: "TipoDeDocumentos",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Monedas_Codigo_Nombre",
                schema: "identity",
                table: "Monedas",
                columns: new[] { "Codigo", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Nombre",
                schema: "identity",
                table: "Categorias",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MetodosDePago_Nombre",
                schema: "identity",
                table: "MetodosDePago",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MetodosDePago",
                schema: "identity");

            migrationBuilder.DropIndex(
                name: "IX_TipoDeDocumentos_Nombre",
                schema: "identity",
                table: "TipoDeDocumentos");

            migrationBuilder.DropIndex(
                name: "IX_Monedas_Codigo_Nombre",
                schema: "identity",
                table: "Monedas");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_Nombre",
                schema: "identity",
                table: "Categorias");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                schema: "identity",
                table: "TipoDeDocumentos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                schema: "identity",
                table: "Monedas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                schema: "identity",
                table: "Categorias",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
