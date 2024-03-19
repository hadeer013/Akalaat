using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Akalaat.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Prop_Of_Price_To_Item : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Items",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Items");
        }
    }
}
