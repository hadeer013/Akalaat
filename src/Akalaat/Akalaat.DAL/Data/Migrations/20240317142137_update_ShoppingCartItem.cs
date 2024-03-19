using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Akalaat.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_ShoppingCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCart_Customer_Customer_ID",
                table: "ShoppingCart");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCart_Customer_ID",
                table: "ShoppingCart");

            migrationBuilder.DropColumn(
                name: "Customer_ID",
                table: "ShoppingCart");

            migrationBuilder.AddColumn<float>(
                name: "TotalPrice",
                table: "ShoppingCart",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "ShoppingCart");

            migrationBuilder.AddColumn<string>(
                name: "Customer_ID",
                table: "ShoppingCart",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCart_Customer_ID",
                table: "ShoppingCart",
                column: "Customer_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCart_Customer_Customer_ID",
                table: "ShoppingCart",
                column: "Customer_ID",
                principalTable: "Customer",
                principalColumn: "Id");
        }
    }
}
