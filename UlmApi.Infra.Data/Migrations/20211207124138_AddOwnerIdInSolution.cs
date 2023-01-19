using Microsoft.EntityFrameworkCore.Migrations;

namespace UlmApi.Infra.Data.Migrations
{
    public partial class AddOwnerIdInSolution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LICENSE_USER_OwnerId",
                table: "LICENSE");

            migrationBuilder.DropForeignKey(
                name: "FK_REQUEST_LICENSE_APPLICATION_ApplicationId",
                table: "REQUEST_LICENSE");

            migrationBuilder.DropIndex(
                name: "IX_LICENSE_OwnerId",
                table: "LICENSE");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "LICENSE");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "SOLUTION",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationId",
                table: "REQUEST_LICENSE",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_SOLUTION_OwnerId",
                table: "SOLUTION",
                column: "OwnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_REQUEST_LICENSE_APPLICATION_ApplicationId",
                table: "REQUEST_LICENSE",
                column: "ApplicationId",
                principalTable: "APPLICATION",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_REQUEST_LICENSE_APPLICATION_ApplicationId",
                table: "REQUEST_LICENSE");

            migrationBuilder.DropForeignKey(
                name: "FK_SOLUTION_USER_OwnerId",
                table: "SOLUTION");

            migrationBuilder.DropIndex(
                name: "IX_SOLUTION_OwnerId",
                table: "SOLUTION");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "SOLUTION");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationId",
                table: "REQUEST_LICENSE",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "LICENSE",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LICENSE_OwnerId",
                table: "LICENSE",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LICENSE_USER_OwnerId",
                table: "LICENSE",
                column: "OwnerId",
                principalTable: "USER",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_REQUEST_LICENSE_APPLICATION_ApplicationId",
                table: "REQUEST_LICENSE",
                column: "ApplicationId",
                principalTable: "APPLICATION",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
