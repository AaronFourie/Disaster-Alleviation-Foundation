using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10101067_APPR6312_POE_PART_2.Data.Migrations
{
    /// <inheritdoc />
    public partial class ggg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
               name: "MoneyDonation",
               columns: table => new
               {
                   MONEY_DONATION_ID = table.Column<int>(type: "int", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                   AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                   DONOR = table.Column<string>(type: "nvarchar(max)", nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_MoneyDonation", x => x.MONEY_DONATION_ID);
               });
            migrationBuilder.AddColumn<string>(
            name: "USERNAME",
            table: "MoneyDonation",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
