using Microsoft.EntityFrameworkCore.Migrations;

namespace UlmApi.Infra.Data.Migrations
{
    public partial class AddLicenseIdInRequestLicense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LICENSE_REQUEST_LICENSE_RequestId",
                table: "LICENSE");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "REQUEST_LICENSE");

            migrationBuilder.DropColumn(
                name: "LicenseType",
                table: "LICENSE");

            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "LICENSE");

            migrationBuilder.DropColumn(
                name: "Provider",
                table: "LICENSE");

            migrationBuilder.RenameColumn(
                name: "RequestId",
                table: "LICENSE",
                newName: "SolutionId");

            migrationBuilder.RenameIndex(
                name: "IX_LICENSE_RequestId",
                table: "LICENSE",
                newName: "IX_LICENSE_SolutionId");

            migrationBuilder.AddColumn<int>(
                name: "LicenseId",
                table: "REQUEST_LICENSE",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_REQUEST_LICENSE_LicenseId",
                table: "REQUEST_LICENSE",
                column: "LicenseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LICENSE_SOLUTION_SolutionId",
                table: "LICENSE",
                column: "SolutionId",
                principalTable: "SOLUTION",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_REQUEST_LICENSE_LICENSE_LicenseId",
                table: "REQUEST_LICENSE",
                column: "LicenseId",
                principalTable: "LICENSE",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LICENSE_SOLUTION_SolutionId",
                table: "LICENSE");

            migrationBuilder.DropForeignKey(
                name: "FK_REQUEST_LICENSE_LICENSE_LicenseId",
                table: "REQUEST_LICENSE");

            migrationBuilder.DropIndex(
                name: "IX_REQUEST_LICENSE_LicenseId",
                table: "REQUEST_LICENSE");

            migrationBuilder.DropColumn(
                name: "LicenseId",
                table: "REQUEST_LICENSE");

            migrationBuilder.RenameColumn(
                name: "SolutionId",
                table: "LICENSE",
                newName: "RequestId");

            migrationBuilder.RenameIndex(
                name: "IX_LICENSE_SolutionId",
                table: "LICENSE",
                newName: "IX_LICENSE_RequestId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "REQUEST_LICENSE",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenseType",
                table: "LICENSE",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "LICENSE",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Provider",
                table: "LICENSE",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_LICENSE_REQUEST_LICENSE_RequestId",
                table: "LICENSE",
                column: "RequestId",
                principalTable: "REQUEST_LICENSE",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
