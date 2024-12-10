using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrankyFinance.Migrations
{
    /// <inheritdoc />
    public partial class FixPagoConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_Users_PagadorId",
                table: "Pagos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_Users_ReceptorId",
                table: "Pagos");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_Users_PagadorId",
                table: "Pagos",
                column: "PagadorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_Users_ReceptorId",
                table: "Pagos",
                column: "ReceptorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_Users_PagadorId",
                table: "Pagos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_Users_ReceptorId",
                table: "Pagos");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_Users_PagadorId",
                table: "Pagos",
                column: "PagadorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_Users_ReceptorId",
                table: "Pagos",
                column: "ReceptorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
