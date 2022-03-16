using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FundStack.Data.Migrations
{
    public partial class AssetsHistoryTableImproved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPrice",
                table: "AssetsHistory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentPrice",
                table: "AssetsHistory",
                type: "decimal(18,10)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
