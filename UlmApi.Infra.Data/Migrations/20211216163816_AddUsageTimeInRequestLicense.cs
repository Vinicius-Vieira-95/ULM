using Microsoft.EntityFrameworkCore.Migrations;

namespace UlmApi.Infra.Data.Migrations
{
    public partial class AddUsageTimeInRequestLicense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SOLUTION_PRODUCT_ProductId",
                table: "SOLUTION");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "SOLUTION",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "UsageTime",
                table: "REQUEST_LICENSE",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_SOLUTION_PRODUCT_ProductId",
                table: "SOLUTION",
                column: "ProductId",
                principalTable: "PRODUCT",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SOLUTION_PRODUCT_ProductId",
                table: "SOLUTION");

            migrationBuilder.DropColumn(
                name: "UsageTime",
                table: "REQUEST_LICENSE");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "SOLUTION",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SOLUTION_PRODUCT_ProductId",
                table: "SOLUTION",
                column: "ProductId",
                principalTable: "PRODUCT",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
