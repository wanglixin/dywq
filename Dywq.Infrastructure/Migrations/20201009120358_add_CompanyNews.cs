using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dywq.Infrastructure.Migrations
{
    public partial class add_CompanyNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyNews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 150, nullable: true),
                    Logo = table.Column<string>(maxLength: 150, nullable: true),
                    CompanyTypeId = table.Column<int>(nullable: false),
                    IntroduceImage = table.Column<string>(maxLength: 200, nullable: true),
                    Introduce = table.Column<string>(nullable: true),
                    MainBusiness = table.Column<string>(nullable: true),
                    CooperationContent = table.Column<string>(nullable: true),
                    Contact = table.Column<string>(nullable: true),
                    Show = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Sort = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyNews", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyNews");
        }
    }
}
