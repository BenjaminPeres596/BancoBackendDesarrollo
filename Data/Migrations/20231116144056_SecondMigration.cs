using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BancoId",
                table: "Empleado",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BandoId",
                table: "Cliente",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Banco",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RazonSocial = table.Column<string>(type: "text", nullable: false),
                    Telefono = table.Column<int>(type: "integer", nullable: false),
                    Calle = table.Column<string>(type: "text", nullable: false),
                    Numero = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banco", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_BancoId",
                table: "Empleado",
                column: "BancoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_BandoId",
                table: "Cliente",
                column: "BandoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Banco_BandoId",
                table: "Cliente",
                column: "BandoId",
                principalTable: "Banco",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleado_Banco_BancoId",
                table: "Empleado",
                column: "BancoId",
                principalTable: "Banco",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Banco_BandoId",
                table: "Cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleado_Banco_BancoId",
                table: "Empleado");

            migrationBuilder.DropTable(
                name: "Banco");

            migrationBuilder.DropIndex(
                name: "IX_Empleado_BancoId",
                table: "Empleado");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_BandoId",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "BancoId",
                table: "Empleado");

            migrationBuilder.DropColumn(
                name: "BandoId",
                table: "Cliente");
        }
    }
}
