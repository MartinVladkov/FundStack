using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FundStack.Data.Migrations
{
    public partial class PortfolioRemoveUnnecessaryField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialInvestment",
                table: "Portfolios");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "InitialInvestment",
                table: "Portfolios",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
