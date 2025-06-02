using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovimientoGastos.Migrations
{
    /// <inheritdoc />
    public partial class PostgreSQLFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                table: "Gastos",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<int>(
                name: "FondoMonetarioId",
                table: "Gastos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                table: "Depositos",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                table: "Gastos",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<int>(
                name: "FondoId1",
                table: "Gastos",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                table: "Depositos",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

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
    }
}
