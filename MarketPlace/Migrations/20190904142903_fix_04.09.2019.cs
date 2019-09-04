using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketPlace.Migrations
{
    public partial class fix_04092019 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Lots",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lots_OwnerId",
                table: "Lots",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lots_AspNetUsers_OwnerId",
                table: "Lots",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lots_AspNetUsers_OwnerId",
                table: "Lots");

            migrationBuilder.DropIndex(
                name: "IX_Lots_OwnerId",
                table: "Lots");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Lots");
        }
    }
}
