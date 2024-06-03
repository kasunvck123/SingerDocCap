using Microsoft.EntityFrameworkCore.Migrations;

namespace SS.DocCap.Infastructure.Migrations
{
    public partial class metadta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CaptureType",
                table: "DocumentData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Folder",
                table: "DocumentData",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaptureType",
                table: "DocumentData");

            migrationBuilder.DropColumn(
                name: "Folder",
                table: "DocumentData");
        }
    }
}
