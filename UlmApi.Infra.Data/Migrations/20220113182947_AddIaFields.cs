using Microsoft.EntityFrameworkCore.Migrations;

namespace UlmApi.Infra.Data.Migrations
{
    public partial class AddIaFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "REQUEST_LICENSE",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Percentage",
                table: "REQUEST_LICENSE",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Prediction",
                table: "REQUEST_LICENSE",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "REQUEST_LICENSE");

            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "REQUEST_LICENSE");

            migrationBuilder.DropColumn(
                name: "Prediction",
                table: "REQUEST_LICENSE");
        }
    }
}
