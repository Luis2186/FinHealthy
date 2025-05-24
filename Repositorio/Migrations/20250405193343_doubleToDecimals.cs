using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class doubleToDecimals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TipoDeCambio",
                table: "Monedas",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateTable(
                name: "Documento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoDeDocumentoId = table.Column<int>(type: "int", nullable: false),
                    EntidadEmisora = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaDeEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaDeVencimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    urlArchivoAdjunto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Etiqueta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documento_TipoDeDocumentos_TipoDeDocumentoId",
                        column: x => x.TipoDeDocumentoId,
                        principalTable: "TipoDeDocumentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gastos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    MetodoDePagoId = table.Column<int>(type: "int", nullable: false),
                    DocumentoAsociadoId = table.Column<int>(type: "int", nullable: true),
                    MonedaCodigo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FechaDeGasto = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lugar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Etiqueta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    EsFinanciado = table.Column<bool>(type: "bit", nullable: false),
                    CantidadDeCuotas = table.Column<int>(type: "int", nullable: false),
                    EsCompartido = table.Column<bool>(type: "bit", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gastos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gastos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gastos_Documento_DocumentoAsociadoId",
                        column: x => x.DocumentoAsociadoId,
                        principalTable: "Documento",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Gastos_MetodosDePago_MetodoDePagoId",
                        column: x => x.MetodoDePagoId,
                        principalTable: "MetodosDePago",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gastos_Monedas_MonedaCodigo",
                        column: x => x.MonedaCodigo,
                        principalTable: "Monedas",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cuota",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GastoId = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pagado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuota", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cuota_Gastos_GastoId",
                        column: x => x.GastoId,
                        principalTable: "Gastos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GastoCompartido",
                columns: table => new
                {
                    GastoId = table.Column<int>(type: "int", nullable: false),
                    MiembroId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Porcentaje = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoAsignado = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GastoCompartido", x => new { x.GastoId, x.MiembroId });
                    table.ForeignKey(
                        name: "FK_GastoCompartido_AspNetUsers_MiembroId",
                        column: x => x.MiembroId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GastoCompartido_Gastos_GastoId",
                        column: x => x.GastoId,
                        principalTable: "Gastos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cuota_GastoId",
                table: "Cuota",
                column: "GastoId");

            migrationBuilder.CreateIndex(
                name: "IX_Documento_TipoDeDocumentoId",
                table: "Documento",
                column: "TipoDeDocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_GastoCompartido_MiembroId",
                table: "GastoCompartido",
                column: "MiembroId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_CategoriaId",
                table: "Gastos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_DocumentoAsociadoId",
                table: "Gastos",
                column: "DocumentoAsociadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_MetodoDePagoId",
                table: "Gastos",
                column: "MetodoDePagoId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_MonedaCodigo",
                table: "Gastos",
                column: "MonedaCodigo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cuota");

            migrationBuilder.DropTable(
                name: "GastoCompartido");

            migrationBuilder.DropTable(
                name: "Gastos");

            migrationBuilder.DropTable(
                name: "Documento");

            migrationBuilder.AlterColumn<double>(
                name: "TipoDeCambio",
                table: "Monedas",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
