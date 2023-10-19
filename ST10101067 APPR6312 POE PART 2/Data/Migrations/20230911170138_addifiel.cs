using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10101067_APPR6312_POE_PART_2.Data.Migrations
{
    /// <inheritdoc />
    public partial class addifiel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "USERNAME",
                table: "MoneyDonation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "USERNAME",
                table: "GoodsDonation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "USERNAME",
                table: "Disatser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "USERNAME",
                table: "MoneyDonation");

            migrationBuilder.DropColumn(
                name: "USERNAME",
                table: "GoodsDonation");

            migrationBuilder.DropColumn(
                name: "USERNAME",
                table: "Disatser");
        }
    }
}
