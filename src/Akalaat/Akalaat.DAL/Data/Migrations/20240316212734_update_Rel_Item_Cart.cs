using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Akalaat.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_Rel_Item_Cart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_Items_ItemsId",
                table: "ShoppingCartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_ShoppingCart_ShoppingCartsId",
                table: "ShoppingCartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartItems",
                table: "ShoppingCartItems");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartItems_ShoppingCartsId",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ShoppingCart");

            migrationBuilder.RenameColumn(
                name: "ShoppingCartsId",
                table: "ShoppingCartItems",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "ItemsId",
                table: "ShoppingCartItems",
                newName: "ItemId");

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartId",
                table: "ShoppingCartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartItems",
                table: "ShoppingCartItems",
                columns: new[] { "ShoppingCartId", "ItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_ItemId",
                table: "ShoppingCartItems",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_Items_ItemId",
                table: "ShoppingCartItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_ShoppingCart_ShoppingCartId",
                table: "ShoppingCartItems",
                column: "ShoppingCartId",
                principalTable: "ShoppingCart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_Items_ItemId",
                table: "ShoppingCartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_ShoppingCart_ShoppingCartId",
                table: "ShoppingCartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartItems",
                table: "ShoppingCartItems");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartItems_ItemId",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "ShoppingCartItems");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "ShoppingCartItems",
                newName: "ShoppingCartsId");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "ShoppingCartItems",
                newName: "ItemsId");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ShoppingCart",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartItems",
                table: "ShoppingCartItems",
                columns: new[] { "ItemsId", "ShoppingCartsId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_ShoppingCartsId",
                table: "ShoppingCartItems",
                column: "ShoppingCartsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_Items_ItemsId",
                table: "ShoppingCartItems",
                column: "ItemsId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_ShoppingCart_ShoppingCartsId",
                table: "ShoppingCartItems",
                column: "ShoppingCartsId",
                principalTable: "ShoppingCart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
