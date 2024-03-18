using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Akalaat.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToMenuItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Menu_MenuId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "MenuId",
                table: "Items",
                newName: "MenuID");

            migrationBuilder.RenameIndex(
                name: "IX_Items_MenuId",
                table: "Items",
                newName: "IX_Items_MenuID");

            migrationBuilder.AlterColumn<int>(
                name: "MenuID",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryID",
                table: "Items",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_CategoryID",
                table: "Items",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Menu_MenuID",
                table: "Items",
                column: "MenuID",
                principalTable: "Menu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_CategoryID",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Menu_MenuID",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CategoryID",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "MenuID",
                table: "Items",
                newName: "MenuId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_MenuID",
                table: "Items",
                newName: "IX_Items_MenuId");

            migrationBuilder.AlterColumn<int>(
                name: "MenuId",
                table: "Items",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Menu_MenuId",
                table: "Items",
                column: "MenuId",
                principalTable: "Menu",
                principalColumn: "Id");
        }
    }
}
