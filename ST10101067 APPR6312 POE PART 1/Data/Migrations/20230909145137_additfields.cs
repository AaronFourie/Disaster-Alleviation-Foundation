using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10101067_APPR6312_POE_PART_2.Data.Migrations
{
    /// <inheritdoc />
    public partial class additfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DONOR",
                table: "Disatser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DONOR",
                table: "Disatser",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
