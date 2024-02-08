using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class SixthMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Cliente");

            migrationBuilder.RenameColumn(
                name: "Calle",
                table: "Cliente",
                newName: "Usuario");

            migrationBuilder.AddColumn<string>(
                name: "Clave",
                table: "Cliente",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Clave",
                table: "Cliente");

            migrationBuilder.RenameColumn(
                name: "Usuario",
                table: "Cliente",
                newName: "Calle");

            migrationBuilder.AddColumn<int>(
                name: "Numero",
                table: "Cliente",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
