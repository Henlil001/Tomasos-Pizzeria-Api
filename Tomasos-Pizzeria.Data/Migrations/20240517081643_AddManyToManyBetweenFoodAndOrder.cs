using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tomasos_Pizzeria.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddManyToManyBetweenFoodAndOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Orders_OrderID",
                table: "Foods");

            migrationBuilder.DropIndex(
                name: "IX_Foods_OrderID",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "OrderID",
                table: "Foods");

            migrationBuilder.CreateTable(
                name: "FoodOrder",
                columns: table => new
                {
                    FoodsFoodID = table.Column<int>(type: "int", nullable: false),
                    OrdersOrderID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodOrder", x => new { x.FoodsFoodID, x.OrdersOrderID });
                    table.ForeignKey(
                        name: "FK_FoodOrder_Foods_FoodsFoodID",
                        column: x => x.FoodsFoodID,
                        principalTable: "Foods",
                        principalColumn: "FoodID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodOrder_Orders_OrdersOrderID",
                        column: x => x.OrdersOrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodOrder_OrdersOrderID",
                table: "FoodOrder",
                column: "OrdersOrderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodOrder");

            migrationBuilder.AddColumn<int>(
                name: "OrderID",
                table: "Foods",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Foods_OrderID",
                table: "Foods",
                column: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_Orders_OrderID",
                table: "Foods",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID");
        }
    }
}
