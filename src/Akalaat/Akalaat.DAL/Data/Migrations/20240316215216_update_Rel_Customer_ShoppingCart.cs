using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Akalaat.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_Rel_Customer_ShoppingCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShoppingCart_ID",
                table: "Customer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ShoppingCart_ID",
                table: "Customer",
                column: "ShoppingCart_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_ShoppingCart_ShoppingCart_ID",
                table: "Customer",
                column: "ShoppingCart_ID",
                principalTable: "ShoppingCart",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_ShoppingCart_ShoppingCart_ID",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_ShoppingCart_ID",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ShoppingCart_ID",
                table: "Customer");
        }
    }
}
