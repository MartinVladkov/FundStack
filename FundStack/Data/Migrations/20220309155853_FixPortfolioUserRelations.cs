using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FundStack.Data.Migrations
{
    public partial class FixPortfolioUserRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Portfolios_PortfolioId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Portfolios_PortfolioId",
                table: "Assets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Portfolios",
                table: "Portfolios");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PortfolioId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Portfolios",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PortfolioId",
                table: "Assets",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Portfolios",
                table: "Portfolios",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Portfolios_PortfolioId",
                table: "Assets",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolios_AspNetUsers_UserId",
                table: "Portfolios",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Portfolios_PortfolioId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Portfolios_AspNetUsers_UserId",
                table: "Portfolios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Portfolios",
                table: "Portfolios");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Portfolios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Portfolios",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "PortfolioId",
                table: "Assets",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Portfolios",
                table: "Portfolios",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PortfolioId",
                table: "AspNetUsers",
                column: "PortfolioId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Portfolios_PortfolioId",
                table: "AspNetUsers",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Portfolios_PortfolioId",
                table: "Assets",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
