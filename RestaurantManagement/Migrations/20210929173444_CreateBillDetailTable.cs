using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantManagement.Migrations
{
    public partial class CreateBillDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BILL_DETAIL",
                columns: table => new
                {
                    BillId = table.Column<Guid>(nullable: false),
                    FoodId = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BILL_DETAIL", x => new { x.BillId, x.FoodId });
                    table.ForeignKey(
                        name: "FK_BILL_DETAIL_BILL_BillId",
                        column: x => x.BillId,
                        principalTable: "BILL",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BILL_DETAIL_FOOD_FoodId",
                        column: x => x.FoodId,
                        principalTable: "FOOD",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BILL_DETAIL_FoodId",
                table: "BILL_DETAIL",
                column: "FoodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BILL_DETAIL");
        }
    }
}
