using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10101067_APPR6312_POE_PART_2.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdditioFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NEW_CATEGORY",
                table: "GoodsDonation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NEW_CATEGORY",
                table: "GoodsDonation");
        }
    }
}
