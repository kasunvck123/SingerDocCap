using Microsoft.EntityFrameworkCore.Migrations;

namespace SS.DocCap.Infastructure.Migrations
{
    public partial class rooturl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RootUrl",
                table: "DocumentData",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RootUrl",
                table: "DocumentData");
        }
    }
}
