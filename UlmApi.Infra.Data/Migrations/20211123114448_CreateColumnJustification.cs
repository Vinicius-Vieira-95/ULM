using Microsoft.EntityFrameworkCore.Migrations;

namespace UlmApi.Infra.Data.Migrations
{
    public partial class CreateColumnJustification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicenseType",
                table: "REQUEST_LICENSE");

            migrationBuilder.AddColumn<string>(
                name: "Justification",
                table: "REQUEST_LICENSE",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Justification",
                table: "REQUEST_LICENSE");

            migrationBuilder.AddColumn<string>(
                name: "LicenseType",
                table: "REQUEST_LICENSE",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
