using Microsoft.EntityFrameworkCore.Migrations;

namespace SS.DocCap.Infastructure.Migrations
{
    public partial class ischeissue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsChequeIssue",
                table: "DocumentData",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChequeIssue",
                table: "DocumentData");
        }
    }
}
