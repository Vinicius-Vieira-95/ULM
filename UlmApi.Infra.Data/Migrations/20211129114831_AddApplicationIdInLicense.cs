using Microsoft.EntityFrameworkCore.Migrations;

namespace UlmApi.Infra.Data.Migrations
{
    public partial class AddApplicationIdInLicense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_REQUEST_LICENSE_LICENSE_LicenseId",
                table: "REQUEST_LICENSE");

            migrationBuilder.AlterColumn<int>(
                name: "LicenseId",
                table: "REQUEST_LICENSE",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "LICENSE",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LICENSE_ApplicationId",
                table: "LICENSE",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_LICENSE_APPLICATION_ApplicationId",
                table: "LICENSE",
                column: "ApplicationId",
                principalTable: "APPLICATION",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_REQUEST_LICENSE_LICENSE_LicenseId",
                table: "REQUEST_LICENSE",
                column: "LicenseId",
                principalTable: "LICENSE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LICENSE_APPLICATION_ApplicationId",
                table: "LICENSE");

            migrationBuilder.DropForeignKey(
                name: "FK_REQUEST_LICENSE_LICENSE_LicenseId",
                table: "REQUEST_LICENSE");

            migrationBuilder.DropIndex(
                name: "IX_LICENSE_ApplicationId",
                table: "LICENSE");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "LICENSE");

            migrationBuilder.AlterColumn<int>(
                name: "LicenseId",
                table: "REQUEST_LICENSE",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_REQUEST_LICENSE_LICENSE_LicenseId",
                table: "REQUEST_LICENSE",
                column: "LicenseId",
                principalTable: "LICENSE",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
