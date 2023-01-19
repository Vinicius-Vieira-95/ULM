using Microsoft.EntityFrameworkCore.Migrations;

namespace UlmApi.Infra.Data.Migrations
{
    public partial class AddProductInSolution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "SOLUTION",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SOLUTION_ProductId",
                table: "SOLUTION",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_SOLUTION_PRODUCT_ProductId",
                table: "SOLUTION",
                column: "ProductId",
                principalTable: "PRODUCT",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SOLUTION_PRODUCT_ProductId",
                table: "SOLUTION");

            migrationBuilder.DropIndex(
                name: "IX_SOLUTION_ProductId",
                table: "SOLUTION");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "SOLUTION");
        }
    }
}
