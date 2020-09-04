using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dywq.Infrastructure.Migrations
{
    public partial class add_default_field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "SiteInfo",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "SiteInfo",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "SiteInfo",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "SiteInfo");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "SiteInfo");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "SiteInfo");
        }
    }
}
