using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovimientoGastos.Data.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionNavegacionFondoMonetario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Depositos_FondosMonetarios_FondoMonetarioId1",
                table: "Depositos");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_FondosMonetarios_FondoMonetarioId1",
                table: "Gastos");

            migrationBuilder.DropIndex(
                name: "IX_Gastos_FondoMonetarioId1",
                table: "Gastos");

            migrationBuilder.DropIndex(
                name: "IX_Depositos_FondoMonetarioId1",
                table: "Depositos");

            migrationBuilder.DropColumn(
                name: "FondoMonetarioId1",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "FondoMonetarioId1",
                table: "Depositos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FondoMonetarioId1",
                table: "Gastos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FondoMonetarioId1",
                table: "Depositos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_FondoMonetarioId1",
                table: "Gastos",
                column: "FondoMonetarioId1");

            migrationBuilder.CreateIndex(
                name: "IX_Depositos_FondoMonetarioId1",
                table: "Depositos",
                column: "FondoMonetarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Depositos_FondosMonetarios_FondoMonetarioId1",
                table: "Depositos",
                column: "FondoMonetarioId1",
                principalTable: "FondosMonetarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_FondosMonetarios_FondoMonetarioId1",
                table: "Gastos",
                column: "FondoMonetarioId1",
                principalTable: "FondosMonetarios",
                principalColumn: "Id");
        }
    }
}
