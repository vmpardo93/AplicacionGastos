using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovimientoGastos.Data.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionMapeoFondoMonetarioId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_FondosMonetarios_FondoMonetarioId",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Fondos_FondoId",
                table: "Gastos");

            migrationBuilder.DropIndex(
                name: "IX_Gastos_FondoMonetarioId",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "FondoMonetarioId",
                table: "Gastos");

            migrationBuilder.AlterColumn<int>(
                name: "FondoId",
                table: "Gastos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FondoId1",
                table: "Gastos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_FondoId1",
                table: "Gastos",
                column: "FondoId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_FondosMonetarios_FondoId",
                table: "Gastos",
                column: "FondoId",
                principalTable: "FondosMonetarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Fondos_FondoId1",
                table: "Gastos",
                column: "FondoId1",
                principalTable: "Fondos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_FondosMonetarios_FondoId",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Fondos_FondoId1",
                table: "Gastos");

            migrationBuilder.DropIndex(
                name: "IX_Gastos_FondoId1",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "FondoId1",
                table: "Gastos");

            migrationBuilder.AlterColumn<int>(
                name: "FondoId",
                table: "Gastos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FondoMonetarioId",
                table: "Gastos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_FondoMonetarioId",
                table: "Gastos",
                column: "FondoMonetarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_FondosMonetarios_FondoMonetarioId",
                table: "Gastos",
                column: "FondoMonetarioId",
                principalTable: "FondosMonetarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Fondos_FondoId",
                table: "Gastos",
                column: "FondoId",
                principalTable: "Fondos",
                principalColumn: "Id");
        }
    }
}
