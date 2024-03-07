using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Akalaat.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class Intialdatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Resturant_ID",
                table: "Vendor",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Address_Book_ID",
                table: "Customer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Dish",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dish_image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dish", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Item_Size",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Size = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item_Size", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Resturant_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Mood",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mood", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Start_Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Customer_ID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Reservation_Customer_Customer_ID",
                        column: x => x.Customer_ID,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "District",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_District", x => x.ID);
                    table.ForeignKey(
                        name: "FK_District_City_City_ID",
                        column: x => x.City_ID,
                        principalTable: "City",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category_image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Menu_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Category_Menu_Menu_ID",
                        column: x => x.Menu_ID,
                        principalTable: "Menu",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Item_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image_URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsOffer = table.Column<bool>(type: "bit", nullable: false),
                    MenuID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Item_ID);
                    table.ForeignKey(
                        name: "FK_Item_Menu_MenuID",
                        column: x => x.MenuID,
                        principalTable: "Menu",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Offer",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Offer_image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Menu_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offer", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Offer_Menu_Menu_ID",
                        column: x => x.Menu_ID,
                        principalTable: "Menu",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Resturant",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo_URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cover_URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Vendor_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Menu_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resturant", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Resturant_Menu_Menu_ID",
                        column: x => x.Menu_ID,
                        principalTable: "Menu",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Resturant_Vendor_Vendor_ID",
                        column: x => x.Vendor_ID,
                        principalTable: "Vendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Region_District_District_ID",
                        column: x => x.District_ID,
                        principalTable: "District",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Extra",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Item_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Extra", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Extra_Item_Item_ID",
                        column: x => x.Item_ID,
                        principalTable: "Item",
                        principalColumn: "Item_ID");
                });

            migrationBuilder.CreateTable(
                name: "Menu_Item_Size",
                columns: table => new
                {
                    Item_ID = table.Column<int>(type: "int", nullable: false),
                    Item_Size_ID = table.Column<int>(type: "int", nullable: false),
                    Size_Image_URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu_Item_Size", x => new { x.Item_ID, x.Item_Size_ID });
                    table.ForeignKey(
                        name: "FK_Menu_Item_Size_Item_Item_ID",
                        column: x => x.Item_ID,
                        principalTable: "Item",
                        principalColumn: "Item_ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Menu_Item_Size_Item_Size_Item_Size_ID",
                        column: x => x.Item_Size_ID,
                        principalTable: "Item_Size",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Arrival_Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total_Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total_Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Customer_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Item_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Order_Customer_Customer_ID",
                        column: x => x.Customer_ID,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Order_Item_Item_ID",
                        column: x => x.Item_ID,
                        principalTable: "Item",
                        principalColumn: "Item_ID");
                });

            migrationBuilder.CreateTable(
                name: "Items_in_Offer",
                columns: table => new
                {
                    Offer_ID = table.Column<int>(type: "int", nullable: false),
                    Item_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items_in_Offer", x => new { x.Offer_ID, x.Item_ID });
                    table.ForeignKey(
                        name: "FK_Items_in_Offer_Item_Item_ID",
                        column: x => x.Item_ID,
                        principalTable: "Item",
                        principalColumn: "Item_ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Items_in_Offer_Offer_Offer_ID",
                        column: x => x.Offer_ID,
                        principalTable: "Offer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Resturant_Dish",
                columns: table => new
                {
                    Resturant_ID = table.Column<int>(type: "int", nullable: false),
                    Dish_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resturant_Dish", x => new { x.Resturant_ID, x.Dish_ID });
                    table.ForeignKey(
                        name: "FK_Resturant_Dish_Dish_Dish_ID",
                        column: x => x.Dish_ID,
                        principalTable: "Dish",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Resturant_Dish_Resturant_Resturant_ID",
                        column: x => x.Resturant_ID,
                        principalTable: "Resturant",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Resturant_Mood",
                columns: table => new
                {
                    Resturant_ID = table.Column<int>(type: "int", nullable: false),
                    Mood_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resturant_Mood", x => new { x.Resturant_ID, x.Mood_ID });
                    table.ForeignKey(
                        name: "FK_Resturant_Mood_Mood_Mood_ID",
                        column: x => x.Mood_ID,
                        principalTable: "Mood",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Resturant_Mood_Resturant_Resturant_ID",
                        column: x => x.Resturant_ID,
                        principalTable: "Resturant",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    No_of_Likes = table.Column<int>(type: "int", nullable: false),
                    Customer_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Resturant_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Review_Customer_Customer_ID",
                        column: x => x.Customer_ID,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Review_Resturant_Resturant_ID",
                        column: x => x.Resturant_ID,
                        principalTable: "Resturant",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Address_Book",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Customer_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Region_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address_Book", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Address_Book_Customer_Customer_ID",
                        column: x => x.Customer_ID,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Address_Book_Region_Region_ID",
                        column: x => x.Region_ID,
                        principalTable: "Region",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    Branch_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDineIn = table.Column<bool>(type: "bit", nullable: false),
                    IsDelivery = table.Column<bool>(type: "bit", nullable: false),
                    Open_Hour = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Close_Hour = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estimated_Delivery_Time = table.Column<int>(type: "int", nullable: false),
                    Resturant_ID = table.Column<int>(type: "int", nullable: false),
                    Region_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.Branch_ID);
                    table.ForeignKey(
                        name: "FK_Branch_Region_Region_ID",
                        column: x => x.Region_ID,
                        principalTable: "Region",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Branch_Resturant_Resturant_ID",
                        column: x => x.Resturant_ID,
                        principalTable: "Resturant",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AvailableDeliveryAreas",
                columns: table => new
                {
                    Region_ID = table.Column<int>(type: "int", nullable: false),
                    Branch_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailableDeliveryAreas", x => new { x.Branch_ID, x.Region_ID });
                    table.ForeignKey(
                        name: "FK_AvailableDeliveryAreas_Branch_Branch_ID",
                        column: x => x.Branch_ID,
                        principalTable: "Branch",
                        principalColumn: "Branch_ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AvailableDeliveryAreas_Region_Region_ID",
                        column: x => x.Region_ID,
                        principalTable: "Region",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Branch_Reservation",
                columns: table => new
                {
                    Reservation_ID = table.Column<int>(type: "int", nullable: false),
                    Branch_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch_Reservation", x => new { x.Branch_ID, x.Reservation_ID });
                    table.ForeignKey(
                        name: "FK_Branch_Reservation_Branch_Branch_ID",
                        column: x => x.Branch_ID,
                        principalTable: "Branch",
                        principalColumn: "Branch_ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Branch_Reservation_Reservation_Reservation_ID",
                        column: x => x.Reservation_ID,
                        principalTable: "Reservation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_Resturant_ID",
                table: "Vendor",
                column: "Resturant_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Address_Book_ID",
                table: "Customer",
                column: "Address_Book_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Address_Book_Customer_ID",
                table: "Address_Book",
                column: "Customer_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Address_Book_Region_ID",
                table: "Address_Book",
                column: "Region_ID");

            migrationBuilder.CreateIndex(
                name: "IX_AvailableDeliveryAreas_Region_ID",
                table: "AvailableDeliveryAreas",
                column: "Region_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_Region_ID",
                table: "Branch",
                column: "Region_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_Resturant_ID",
                table: "Branch",
                column: "Resturant_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_Reservation_Reservation_ID",
                table: "Branch_Reservation",
                column: "Reservation_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Menu_ID",
                table: "Category",
                column: "Menu_ID");

            migrationBuilder.CreateIndex(
                name: "IX_District_City_ID",
                table: "District",
                column: "City_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Extra_Item_ID",
                table: "Extra",
                column: "Item_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Item_MenuID",
                table: "Item",
                column: "MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_Items_in_Offer_Item_ID",
                table: "Items_in_Offer",
                column: "Item_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Item_Size_Item_Size_ID",
                table: "Menu_Item_Size",
                column: "Item_Size_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_Menu_ID",
                table: "Offer",
                column: "Menu_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Customer_ID",
                table: "Order",
                column: "Customer_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Item_ID",
                table: "Order",
                column: "Item_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Region_District_ID",
                table: "Region",
                column: "District_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_Customer_ID",
                table: "Reservation",
                column: "Customer_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Resturant_Menu_ID",
                table: "Resturant",
                column: "Menu_ID",
                unique: true,
                filter: "[Menu_ID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Resturant_Vendor_ID",
                table: "Resturant",
                column: "Vendor_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Resturant_Dish_Dish_ID",
                table: "Resturant_Dish",
                column: "Dish_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Resturant_Mood_Mood_ID",
                table: "Resturant_Mood",
                column: "Mood_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Review_Customer_ID",
                table: "Review",
                column: "Customer_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Review_Resturant_ID",
                table: "Review",
                column: "Resturant_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Address_Book_Address_Book_ID",
                table: "Customer",
                column: "Address_Book_ID",
                principalTable: "Address_Book",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendor_Resturant_Resturant_ID",
                table: "Vendor",
                column: "Resturant_ID",
                principalTable: "Resturant",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Address_Book_Address_Book_ID",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendor_Resturant_Resturant_ID",
                table: "Vendor");

            migrationBuilder.DropTable(
                name: "Address_Book");

            migrationBuilder.DropTable(
                name: "AvailableDeliveryAreas");

            migrationBuilder.DropTable(
                name: "Branch_Reservation");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Extra");

            migrationBuilder.DropTable(
                name: "Items_in_Offer");

            migrationBuilder.DropTable(
                name: "Menu_Item_Size");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Resturant_Dish");

            migrationBuilder.DropTable(
                name: "Resturant_Mood");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "Offer");

            migrationBuilder.DropTable(
                name: "Item_Size");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Dish");

            migrationBuilder.DropTable(
                name: "Mood");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropTable(
                name: "Resturant");

            migrationBuilder.DropTable(
                name: "District");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropIndex(
                name: "IX_Vendor_Resturant_ID",
                table: "Vendor");

            migrationBuilder.DropIndex(
                name: "IX_Customer_Address_Book_ID",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Resturant_ID",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "Address_Book_ID",
                table: "Customer");
        }
    }
}
