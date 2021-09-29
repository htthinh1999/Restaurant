using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantManagement.Migrations
{
    public partial class FoodConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FOOD",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    UnitPrice = table.Column<int>(nullable: false),
                    Category = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true, defaultValue: "None."),
                    ImageURL = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FOOD", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FOOD");
        }
    }
}
