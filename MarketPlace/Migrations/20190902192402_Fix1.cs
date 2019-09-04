using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketPlace.Migrations
{
    public partial class Fix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "lastUp",
                table: "Lots",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lastUp",
                table: "Lots");
        }
    }
}
