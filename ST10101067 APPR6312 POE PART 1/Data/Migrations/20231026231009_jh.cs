using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10101067_APPR6312_POE_PART_2.Data.Migrations
{
    /// <inheritdoc />
    public partial class jh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "Disatser",
               columns: table => new
               {
                   DISTATER_ID = table.Column<int>(type: "int", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   STARTDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                   ENDDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                   LOCATION = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   AID_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   DONOR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   ISACTIVE = table.Column<bool>(type: "int", nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Disatser", x => x.DISTATER_ID);
               });

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
            migrationBuilder.DropTable(
                name: "Disatser");

            migrationBuilder.DropColumn(
               name: "USERNAME",
               table: "Disatser");
        }
    }
}
