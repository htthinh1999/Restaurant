using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantManagement.Migrations
{
    public partial class Order_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ODER_TABLE",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<Guid>(nullable: false),
                    TableID = table.Column<int>(nullable: false),
                    From = table.Column<DateTime>(nullable: false),
                    To = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ODER_TABLE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "CUSTOMER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TableID",
                        column: x => x.TableID,
                        principalTable: "TABLE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ODER_TABLE_CustomerID",
                table: "ODER_TABLE",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_ODER_TABLE_TableID",
                table: "ODER_TABLE",
                column: "TableID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ODER_TABLE");
        }
    }
}
