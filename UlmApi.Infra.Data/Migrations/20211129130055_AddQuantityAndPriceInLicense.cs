using Microsoft.EntityFrameworkCore.Migrations;

namespace UlmApi.Infra.Data.Migrations
{
    public partial class AddQuantityAndPriceInLicense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LICENSE_APPLICATION_ApplicationId",
                table: "LICENSE");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationId",
                table: "LICENSE",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "LICENSE",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "LICENSE",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_LICENSE_APPLICATION_ApplicationId",
                table: "LICENSE",
                column: "ApplicationId",
                principalTable: "APPLICATION",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LICENSE_APPLICATION_ApplicationId",
                table: "LICENSE");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "LICENSE");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "LICENSE");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationId",
                table: "LICENSE",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LICENSE_APPLICATION_ApplicationId",
                table: "LICENSE",
                column: "ApplicationId",
                principalTable: "APPLICATION",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
