using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10101067_APPR6312_POE_PART_2.Data.Migrations
{
    /// <inheritdoc />
    public partial class jhjh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MoneyAllocation",
                columns: table => new
                {
                    MoneyAllocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisasterId = table.Column<int>(type: "int", nullable: false),
                    AllocationAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AllocationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyAllocation", x => x.MoneyAllocationId);
                    table.ForeignKey(
                        name: "FK_MoneyAllocation_Disatser_DisasterId",
                        column: x => x.DisasterId,
                        principalTable: "Disatser",
                        principalColumn: "DISTATER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoneyAllocation_DisasterId",
                table: "MoneyAllocation",
                column: "DisasterId");


            migrationBuilder.AddColumn<string>(
                name: "AidType",
                table: "MoneyAllocation",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                defaultValue: "");

            migrationBuilder.DropForeignKey(
                name: "FK_MoneyAllocation_Disatser_DisasterId",
                table: "MoneyAllocation");

            migrationBuilder.DropIndex(
                name: "IX_MoneyAllocation_DisasterId",
                table: "MoneyAllocation");

            migrationBuilder.DropColumn(
                name: "DisasterId",
                table: "MoneyAllocation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisasterId",
                table: "MoneyAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MoneyAllocation_DisasterId",
                table: "MoneyAllocation",
                column: "DisasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_MoneyAllocation_Disatser_DisasterId",
                table: "MoneyAllocation",
                column: "DisasterId",
                principalTable: "Disatser",
                principalColumn: "DISTATER_ID",
                onDelete: ReferentialAction.Cascade);
        }

        
    }
}
