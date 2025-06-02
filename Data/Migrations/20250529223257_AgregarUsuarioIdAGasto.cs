using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovimientoGastos.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarUsuarioIdAGasto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Gastos",
                type: "varchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Gastos");
        }
    }
}
