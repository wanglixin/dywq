using Microsoft.EntityFrameworkCore.Migrations;

namespace Dywq.Infrastructure.Migrations
{
    public partial class add_Financing_Describe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Describe",
                table: "Financing",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Describe",
                table: "Financing");
        }
    }
}
