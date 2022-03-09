using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FundStack.Data.Migrations
{
    public partial class UserPortfolioRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PortfolioId",
                table: "AspNetUsers");

           // migrationBuilder.DropColumn(
           //     name: "Discriminator",
           //     table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "PortfolioId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PortfolioId",
                table: "AspNetUsers",
                column: "PortfolioId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PortfolioId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "PortfolioId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PortfolioId",
                table: "AspNetUsers",
                column: "PortfolioId",
                unique: true,
                filter: "[PortfolioId] IS NOT NULL");
        }
    }
}
