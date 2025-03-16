using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class Grupounoamuchos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GruposDeGasto_UsuarioAdministradorId",
                schema: "identity",
                table: "GruposDeGasto");

            migrationBuilder.RenameTable(
                name: "UsuarioGrupos",
                schema: "identity",
                newName: "UsuarioGrupos");

            migrationBuilder.RenameTable(
                name: "TipoDeDocumentos",
                schema: "identity",
                newName: "TipoDeDocumentos");

            migrationBuilder.RenameTable(
                name: "SubCategorias",
                schema: "identity",
                newName: "SubCategorias");

            migrationBuilder.RenameTable(
                name: "SolcitudesUnionGrupo",
                schema: "identity",
                newName: "SolcitudesUnionGrupo");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                schema: "identity",
                newName: "RefreshTokens");

            migrationBuilder.RenameTable(
                name: "Notificaciones",
                schema: "identity",
                newName: "Notificaciones");

            migrationBuilder.RenameTable(
                name: "Monedas",
                schema: "identity",
                newName: "Monedas");

            migrationBuilder.RenameTable(
                name: "MetodosDePago",
                schema: "identity",
                newName: "MetodosDePago");

            migrationBuilder.RenameTable(
                name: "GruposDeGasto",
                schema: "identity",
                newName: "GruposDeGasto");

            migrationBuilder.RenameTable(
                name: "Categorias",
                schema: "identity",
                newName: "Categorias");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "identity",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "identity",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "identity",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "identity",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "identity",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "identity",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "identity",
                newName: "AspNetRoleClaims");

            migrationBuilder.CreateIndex(
                name: "IX_GruposDeGasto_UsuarioAdministradorId",
                table: "GruposDeGasto",
                column: "UsuarioAdministradorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GruposDeGasto_UsuarioAdministradorId",
                table: "GruposDeGasto");

            migrationBuilder.EnsureSchema(
                name: "identity");

            migrationBuilder.RenameTable(
                name: "UsuarioGrupos",
                newName: "UsuarioGrupos",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "TipoDeDocumentos",
                newName: "TipoDeDocumentos",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "SubCategorias",
                newName: "SubCategorias",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "SolcitudesUnionGrupo",
                newName: "SolcitudesUnionGrupo",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                newName: "RefreshTokens",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "Notificaciones",
                newName: "Notificaciones",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "Monedas",
                newName: "Monedas",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "MetodosDePago",
                newName: "MetodosDePago",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "GruposDeGasto",
                newName: "GruposDeGasto",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "Categorias",
                newName: "Categorias",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "AspNetUserTokens",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "AspNetUsers",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "AspNetUserRoles",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "AspNetUserLogins",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "AspNetUserClaims",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "AspNetRoles",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "AspNetRoleClaims",
                newSchema: "identity");

            migrationBuilder.CreateIndex(
                name: "IX_GruposDeGasto_UsuarioAdministradorId",
                schema: "identity",
                table: "GruposDeGasto",
                column: "UsuarioAdministradorId",
                unique: true);
        }
    }
}
