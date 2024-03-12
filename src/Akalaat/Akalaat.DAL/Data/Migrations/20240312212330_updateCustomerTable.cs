using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Akalaat.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateCustomerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_AddressBooks_Address_Book_ID",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_Address_Book_ID",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_AddressBooks_Customer_ID",
                table: "AddressBooks");

            migrationBuilder.DropColumn(
                name: "Address_Book_ID",
                table: "Customer");

            migrationBuilder.CreateIndex(
                name: "IX_AddressBooks_Customer_ID",
                table: "AddressBooks",
                column: "Customer_ID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AddressBooks_Customer_ID",
                table: "AddressBooks");

            migrationBuilder.AddColumn<int>(
                name: "Address_Book_ID",
                table: "Customer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Address_Book_ID",
                table: "Customer",
                column: "Address_Book_ID");

            migrationBuilder.CreateIndex(
                name: "IX_AddressBooks_Customer_ID",
                table: "AddressBooks",
                column: "Customer_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_AddressBooks_Address_Book_ID",
                table: "Customer",
                column: "Address_Book_ID",
                principalTable: "AddressBooks",
                principalColumn: "Id");
        }
    }
}
