using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class EleventhMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoMotivoId",
                table: "Transferencia",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TipoMotivo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoMotivo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transferencia_TipoMotivoId",
                table: "Transferencia",
                column: "TipoMotivoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transferencia_TipoMotivo_TipoMotivoId",
                table: "Transferencia",
                column: "TipoMotivoId",
                principalTable: "TipoMotivo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transferencia_TipoMotivo_TipoMotivoId",
                table: "Transferencia");

            migrationBuilder.DropTable(
                name: "TipoMotivo");

            migrationBuilder.DropIndex(
                name: "IX_Transferencia_TipoMotivoId",
                table: "Transferencia");

            migrationBuilder.DropColumn(
                name: "TipoMotivoId",
                table: "Transferencia");
        }
    }
}
