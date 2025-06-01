using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovimientoGastos.Data.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionFondoMonetarioId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Depositos_FondosMonetarios_FondoId",
                table: "Depositos");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_FondosMonetarios_FondoId",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_GastosDetalle_TiposGasto_TipoGastoId",
                table: "GastosDetalle");

            migrationBuilder.RenameColumn(
                name: "FondoId",
                table: "Depositos",
                newName: "FondoMonetarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Depositos_FondoId",
                table: "Depositos",
                newName: "IX_Depositos_FondoMonetarioId");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "GastosDetalle",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoGastoId1",
                table: "GastosDetalle",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Observaciones",
                table: "Gastos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<int>(
                name: "FondoId",
                table: "Gastos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Comercio",
                table: "Gastos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "FondoMonetarioId",
                table: "Gastos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FondoMonetarioId1",
                table: "Gastos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroDoc",
                table: "Gastos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Fondos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SaldoInicial = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fondos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GastosDetalle_TipoGastoId1",
                table: "GastosDetalle",
                column: "TipoGastoId1");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_FondoMonetarioId",
                table: "Gastos",
                column: "FondoMonetarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_FondoMonetarioId1",
                table: "Gastos",
                column: "FondoMonetarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Depositos_FondosMonetarios_FondoMonetarioId",
                table: "Depositos",
                column: "FondoMonetarioId",
                principalTable: "FondosMonetarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_FondosMonetarios_FondoMonetarioId",
                table: "Gastos",
                column: "FondoMonetarioId",
                principalTable: "FondosMonetarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_FondosMonetarios_FondoMonetarioId1",
                table: "Gastos",
                column: "FondoMonetarioId1",
                principalTable: "FondosMonetarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Fondos_FondoId",
                table: "Gastos",
                column: "FondoId",
                principalTable: "Fondos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GastosDetalle_TiposGasto_TipoGastoId",
                table: "GastosDetalle",
                column: "TipoGastoId",
                principalTable: "TiposGasto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GastosDetalle_TiposGasto_TipoGastoId1",
                table: "GastosDetalle",
                column: "TipoGastoId1",
                principalTable: "TiposGasto",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Depositos_FondosMonetarios_FondoMonetarioId",
                table: "Depositos");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_FondosMonetarios_FondoMonetarioId",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_FondosMonetarios_FondoMonetarioId1",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Fondos_FondoId",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_GastosDetalle_TiposGasto_TipoGastoId",
                table: "GastosDetalle");

            migrationBuilder.DropForeignKey(
                name: "FK_GastosDetalle_TiposGasto_TipoGastoId1",
                table: "GastosDetalle");

            migrationBuilder.DropTable(
                name: "Fondos");

            migrationBuilder.DropIndex(
                name: "IX_GastosDetalle_TipoGastoId1",
                table: "GastosDetalle");

            migrationBuilder.DropIndex(
                name: "IX_Gastos_FondoMonetarioId",
                table: "Gastos");

            migrationBuilder.DropIndex(
                name: "IX_Gastos_FondoMonetarioId1",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "GastosDetalle");

            migrationBuilder.DropColumn(
                name: "TipoGastoId1",
                table: "GastosDetalle");

            migrationBuilder.DropColumn(
                name: "FondoMonetarioId",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "FondoMonetarioId1",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "NumeroDoc",
                table: "Gastos");

            migrationBuilder.RenameColumn(
                name: "FondoMonetarioId",
                table: "Depositos",
                newName: "FondoId");

            migrationBuilder.RenameIndex(
                name: "IX_Depositos_FondoMonetarioId",
                table: "Depositos",
                newName: "IX_Depositos_FondoId");

            migrationBuilder.AlterColumn<string>(
                name: "Observaciones",
                table: "Gastos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FondoId",
                table: "Gastos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Comercio",
                table: "Gastos",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddForeignKey(
                name: "FK_Depositos_FondosMonetarios_FondoId",
                table: "Depositos",
                column: "FondoId",
                principalTable: "FondosMonetarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_FondosMonetarios_FondoId",
                table: "Gastos",
                column: "FondoId",
                principalTable: "FondosMonetarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GastosDetalle_TiposGasto_TipoGastoId",
                table: "GastosDetalle",
                column: "TipoGastoId",
                principalTable: "TiposGasto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
