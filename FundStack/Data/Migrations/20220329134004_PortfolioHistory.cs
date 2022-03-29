using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FundStack.Data.Migrations
{
    public partial class PortfolioHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PortfoliosHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PortfolioValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SnapshotDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PortfolioId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfoliosHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortfoliosHistory_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PortfoliosHistory_PortfolioId",
                table: "PortfoliosHistory",
                column: "PortfolioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PortfoliosHistory");
        }
    }
}
