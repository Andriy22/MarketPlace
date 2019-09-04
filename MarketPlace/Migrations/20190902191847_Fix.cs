using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketPlace.Migrations
{
    public partial class Fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lots_Categories_CategoryID",
                table: "Lots");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "Lots",
                newName: "categoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Lots_CategoryID",
                table: "Lots",
                newName: "IX_Lots_categoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lots_Categories_categoryID",
                table: "Lots",
                column: "categoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lots_Categories_categoryID",
                table: "Lots");

            migrationBuilder.RenameColumn(
                name: "categoryID",
                table: "Lots",
                newName: "CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Lots_categoryID",
                table: "Lots",
                newName: "IX_Lots_CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lots_Categories_CategoryID",
                table: "Lots",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
