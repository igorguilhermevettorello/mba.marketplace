using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MBA.Marketplace.Data.Migrations
{
    /// <inheritdoc />
    public partial class AjustesCampoProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "preco",
                table: "Produtos",
                newName: "Preco");

            migrationBuilder.RenameColumn(
                name: "estoque",
                table: "Produtos",
                newName: "Estoque");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Vendedores",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Vendedores");

            migrationBuilder.RenameColumn(
                name: "Preco",
                table: "Produtos",
                newName: "preco");

            migrationBuilder.RenameColumn(
                name: "Estoque",
                table: "Produtos",
                newName: "estoque");
        }
    }
}
