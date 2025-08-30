using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class OptimizacionGastos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GastoCompartido_AspNetUsers_MiembroId",
                table: "GastoCompartido");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GastoCompartido",
                table: "GastoCompartido");

            migrationBuilder.DropColumn(
                name: "EsCompartido",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "EsFijo",
                table: "Gastos");

            migrationBuilder.AlterColumn<bool>(
                name: "EsFinanciado",
                table: "Gastos",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "CantidadDeCuotas",
                table: "Gastos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaFin",
                table: "Gastos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicio",
                table: "Gastos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Periodicidad",
                table: "Gastos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoGasto",
                table: "Gastos",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GastoPrincipalId",
                table: "GastoCompartido",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GastoCompartido",
                table: "GastoCompartido",
                columns: new[] { "GastoPrincipalId", "MiembroId" });

            migrationBuilder.CreateIndex(
                name: "IX_GastoCompartido_GastoId",
                table: "GastoCompartido",
                column: "GastoId");

            migrationBuilder.CreateIndex(
                name: "IX_GastoCompartido_GastoPrincipalId",
                table: "GastoCompartido",
                column: "GastoPrincipalId");

            migrationBuilder.AddForeignKey(
                name: "FK_GastoCompartido_AspNetUsers_MiembroId",
                table: "GastoCompartido",
                column: "MiembroId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GastoCompartido_Gastos_GastoPrincipalId",
                table: "GastoCompartido",
                column: "GastoPrincipalId",
                principalTable: "Gastos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GastoCompartido_AspNetUsers_MiembroId",
                table: "GastoCompartido");

            migrationBuilder.DropForeignKey(
                name: "FK_GastoCompartido_Gastos_GastoPrincipalId",
                table: "GastoCompartido");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GastoCompartido",
                table: "GastoCompartido");

            migrationBuilder.DropIndex(
                name: "IX_GastoCompartido_GastoId",
                table: "GastoCompartido");

            migrationBuilder.DropIndex(
                name: "IX_GastoCompartido_GastoPrincipalId",
                table: "GastoCompartido");

            migrationBuilder.DropColumn(
                name: "FechaFin",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "FechaInicio",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "Periodicidad",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "TipoGasto",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "GastoPrincipalId",
                table: "GastoCompartido");

            migrationBuilder.AlterColumn<bool>(
                name: "EsFinanciado",
                table: "Gastos",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CantidadDeCuotas",
                table: "Gastos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EsCompartido",
                table: "Gastos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EsFijo",
                table: "Gastos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GastoCompartido",
                table: "GastoCompartido",
                columns: new[] { "GastoId", "MiembroId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GastoCompartido_AspNetUsers_MiembroId",
                table: "GastoCompartido",
                column: "MiembroId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
