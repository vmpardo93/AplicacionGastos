using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovimientoGastos.Data.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionRelacionesEntidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Depositos_FondosMonetarios_FondoMonetarioId",
                table: "Depositos");

            migrationBuilder.DropForeignKey(
                name: "FK_GastosDetalle_TiposGasto_TipoGastoId1",
                table: "GastosDetalle");

            migrationBuilder.DropIndex(
                name: "IX_GastosDetalle_TipoGastoId1",
                table: "GastosDetalle");

            migrationBuilder.DropColumn(
                name: "TipoGastoId1",
                table: "GastosDetalle");

            migrationBuilder.AddColumn<int>(
                name: "FondoMonetarioId1",
                table: "Depositos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Depositos_FondoMonetarioId1",
                table: "Depositos",
                column: "FondoMonetarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Depositos_FondosMonetarios_FondoMonetarioId",
                table: "Depositos",
                column: "FondoMonetarioId",
                principalTable: "FondosMonetarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Depositos_FondosMonetarios_FondoMonetarioId1",
                table: "Depositos",
                column: "FondoMonetarioId1",
                principalTable: "FondosMonetarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Depositos_FondosMonetarios_FondoMonetarioId",
                table: "Depositos");

            migrationBuilder.DropForeignKey(
                name: "FK_Depositos_FondosMonetarios_FondoMonetarioId1",
                table: "Depositos");

            migrationBuilder.DropIndex(
                name: "IX_Depositos_FondoMonetarioId1",
                table: "Depositos");

            migrationBuilder.DropColumn(
                name: "FondoMonetarioId1",
                table: "Depositos");

            migrationBuilder.AddColumn<int>(
                name: "TipoGastoId1",
                table: "GastosDetalle",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GastosDetalle_TipoGastoId1",
                table: "GastosDetalle",
                column: "TipoGastoId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Depositos_FondosMonetarios_FondoMonetarioId",
                table: "Depositos",
                column: "FondoMonetarioId",
                principalTable: "FondosMonetarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GastosDetalle_TiposGasto_TipoGastoId1",
                table: "GastosDetalle",
                column: "TipoGastoId1",
                principalTable: "TiposGasto",
                principalColumn: "Id");
        }
    }
}
