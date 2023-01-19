using Microsoft.EntityFrameworkCore.Migrations;

namespace UlmApi.Infra.Data.Migrations
{
    public partial class RemoveUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_REQUEST_LICENSE_USER_RequesterId",
                table: "REQUEST_LICENSE");

            migrationBuilder.DropForeignKey(
                name: "FK_SOLUTION_USER_OwnerId",
                table: "SOLUTION");

            migrationBuilder.DropTable(
                name: "USER");

            migrationBuilder.DropIndex(
                name: "IX_SOLUTION_OwnerId",
                table: "SOLUTION");

            migrationBuilder.DropIndex(
                name: "IX_REQUEST_LICENSE_RequesterId",
                table: "REQUEST_LICENSE");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "SOLUTION",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerName",
                table: "SOLUTION",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RequesterName",
                table: "REQUEST_LICENSE",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerName",
                table: "SOLUTION");

            migrationBuilder.DropColumn(
                name: "RequesterName",
                table: "REQUEST_LICENSE");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "SOLUTION",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "USER",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SOLUTION_OwnerId",
                table: "SOLUTION",
                column: "OwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_REQUEST_LICENSE_RequesterId",
                table: "REQUEST_LICENSE",
                column: "RequesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_REQUEST_LICENSE_USER_RequesterId",
                table: "REQUEST_LICENSE",
                column: "RequesterId",
                principalTable: "USER",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SOLUTION_USER_OwnerId",
                table: "SOLUTION",
                column: "OwnerId",
                principalTable: "USER",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
