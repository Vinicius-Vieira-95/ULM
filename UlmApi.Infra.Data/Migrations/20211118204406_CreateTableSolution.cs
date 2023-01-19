using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace UlmApi.Infra.Data.Migrations
{
    public partial class CreateTableSolution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SolutionId",
                table: "REQUEST_LICENSE",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SOLUTION",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOLUTION", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_REQUEST_LICENSE_SolutionId",
                table: "REQUEST_LICENSE",
                column: "SolutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_REQUEST_LICENSE_SOLUTION_SolutionId",
                table: "REQUEST_LICENSE",
                column: "SolutionId",
                principalTable: "SOLUTION",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_REQUEST_LICENSE_SOLUTION_SolutionId",
                table: "REQUEST_LICENSE");

            migrationBuilder.DropTable(
                name: "SOLUTION");

            migrationBuilder.DropIndex(
                name: "IX_REQUEST_LICENSE_SolutionId",
                table: "REQUEST_LICENSE");

            migrationBuilder.DropColumn(
                name: "SolutionId",
                table: "REQUEST_LICENSE");
        }
    }
}
