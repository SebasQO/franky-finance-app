using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrankyFinance.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToGastoModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Gastos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_GroupId",
                table: "Gastos",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Grupos_GroupId",
                table: "Gastos",
                column: "GroupId",
                principalTable: "Grupos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Grupos_GroupId",
                table: "Gastos");

            migrationBuilder.DropIndex(
                name: "IX_Gastos_GroupId",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Gastos");

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Grupos_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_GroupId",
                table: "Expenses",
                column: "GroupId");
        }
    }
}
