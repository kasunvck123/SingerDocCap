using Microsoft.EntityFrameworkCore.Migrations;

namespace SS.DocCap.Infastructure.Migrations
{
    public partial class userprop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BranchCode",
                table: "DocumentData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BranchID",
                table: "DocumentData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BranchName",
                table: "DocumentData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "DocumentData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeType",
                table: "DocumentData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HierachyID",
                table: "DocumentData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "DocumentData",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchCode",
                table: "DocumentData");

            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "DocumentData");

            migrationBuilder.DropColumn(
                name: "BranchName",
                table: "DocumentData");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "DocumentData");

            migrationBuilder.DropColumn(
                name: "EmployeeType",
                table: "DocumentData");

            migrationBuilder.DropColumn(
                name: "HierachyID",
                table: "DocumentData");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DocumentData");
        }
    }
}
