using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FundStack.Data.Migrations
{
    public partial class AssetsHistoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetsHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    BuyPrice = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    BuyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InvestedMoney = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    CurrentPrice = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    SellPrice = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    SellDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfitLoss = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProfitLossPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    PortfolioId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetsHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetsHistory_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetsHistory_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetsHistory_PortfolioId",
                table: "AssetsHistory",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetsHistory_TypeId",
                table: "AssetsHistory",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetsHistory");
        }
    }
}
