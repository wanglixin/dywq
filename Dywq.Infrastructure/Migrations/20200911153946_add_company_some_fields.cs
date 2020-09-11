using Microsoft.EntityFrameworkCore.Migrations;

namespace Dywq.Infrastructure.Migrations
{
    public partial class add_company_some_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CooperationContent",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Introduce",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IntroduceImage",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainBusiness",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Show",
                table: "Company",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Sort",
                table: "Company",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Company",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "CooperationContent",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Introduce",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "IntroduceImage",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "MainBusiness",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Show",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Sort",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Company");
        }
    }
}
