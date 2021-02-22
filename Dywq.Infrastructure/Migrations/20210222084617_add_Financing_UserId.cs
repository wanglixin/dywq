﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Dywq.Infrastructure.Migrations
{
    public partial class add_Financing_UserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Financing",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Financing");
        }
    }
}
