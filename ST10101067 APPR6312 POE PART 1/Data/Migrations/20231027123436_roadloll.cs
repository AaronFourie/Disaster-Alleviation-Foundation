using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10101067_APPR6312_POE_PART_2.Data.Migrations
{
    /// <inheritdoc />
    public partial class roadloll : Migration
    {
        /// <inheritdoc />
        /// 
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoodsPurchase",
                columns: table => new
                {
                    GoodsPurchaseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoodsPurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ITEM_COUNT = table.Column<int>(type: "int", nullable: false),
                    CATEGORY = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsPurchase", x => x.GoodsPurchaseID);
                });


            migrationBuilder.AddColumn<decimal>(
                name: "GoodsTotalPrice",
                table: "GoodsPurchase",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "GoodsPurchase");

            migrationBuilder.DropColumn(
                name: "GoodsTotalPrice",
                table: "GoodsPurchase");
        }
    }
}
