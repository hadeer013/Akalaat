using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Akalaat.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class addBranchDeliveryAreaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Available_Delivery_Area_Branches_BranchId",
                table: "Available_Delivery_Area");

            migrationBuilder.DropForeignKey(
                name: "FK_Available_Delivery_Area_Regions_RegionId",
                table: "Available_Delivery_Area");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Available_Delivery_Area",
                table: "Available_Delivery_Area");

            migrationBuilder.RenameTable(
                name: "Available_Delivery_Area",
                newName: "AvailableDeliveryAreas");

            migrationBuilder.RenameIndex(
                name: "IX_Available_Delivery_Area_RegionId",
                table: "AvailableDeliveryAreas",
                newName: "IX_AvailableDeliveryAreas_RegionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AvailableDeliveryAreas",
                table: "AvailableDeliveryAreas",
                columns: new[] { "BranchId", "RegionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AvailableDeliveryAreas_Branches_BranchId",
                table: "AvailableDeliveryAreas",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_AvailableDeliveryAreas_Regions_RegionId",
                table: "AvailableDeliveryAreas",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvailableDeliveryAreas_Branches_BranchId",
                table: "AvailableDeliveryAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_AvailableDeliveryAreas_Regions_RegionId",
                table: "AvailableDeliveryAreas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AvailableDeliveryAreas",
                table: "AvailableDeliveryAreas");

            migrationBuilder.RenameTable(
                name: "AvailableDeliveryAreas",
                newName: "Available_Delivery_Area");

            migrationBuilder.RenameIndex(
                name: "IX_AvailableDeliveryAreas_RegionId",
                table: "Available_Delivery_Area",
                newName: "IX_Available_Delivery_Area_RegionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Available_Delivery_Area",
                table: "Available_Delivery_Area",
                columns: new[] { "BranchId", "RegionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Available_Delivery_Area_Branches_BranchId",
                table: "Available_Delivery_Area",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Available_Delivery_Area_Regions_RegionId",
                table: "Available_Delivery_Area",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
