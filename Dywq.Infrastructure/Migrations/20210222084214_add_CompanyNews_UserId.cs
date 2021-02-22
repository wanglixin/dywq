using Microsoft.EntityFrameworkCore.Migrations;

namespace Dywq.Infrastructure.Migrations
{
    public partial class add_CompanyNews_UserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CompanyNews",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CompanyNews");
        }
    }
}
