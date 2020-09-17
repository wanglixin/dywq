using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dywq.Infrastructure.Migrations
{
    public partial class add_policyArticle_Guestbook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guestbook",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                    UserId = table.Column<int>(maxLength: 200, nullable: false),
                    Content = table.Column<string>(maxLength: 1000, nullable: true),
                    Type = table.Column<int>(nullable: false),
                    ReplyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guestbook", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PolicyArticle",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ThemeTitle = table.Column<string>(maxLength: 200, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    PolicyTypeId = table.Column<int>(nullable: false),
                    Show = table.Column<bool>(nullable: false),
                    Sort = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyArticle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PolicyType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Sort = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyType", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Guestbook");

            migrationBuilder.DropTable(
                name: "PolicyArticle");

            migrationBuilder.DropTable(
                name: "PolicyType");
        }
    }
}
