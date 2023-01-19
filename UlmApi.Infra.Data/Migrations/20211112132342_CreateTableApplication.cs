using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace UlmApi.Infra.Data.Migrations
{
    public partial class CreateTableApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "REQUEST_LICENSE",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "APPLICATION",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APPLICATION", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_REQUEST_LICENSE_ApplicationId",
                table: "REQUEST_LICENSE",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_REQUEST_LICENSE_APPLICATION_ApplicationId",
                table: "REQUEST_LICENSE",
                column: "ApplicationId",
                principalTable: "APPLICATION",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_REQUEST_LICENSE_APPLICATION_ApplicationId",
                table: "REQUEST_LICENSE");

            migrationBuilder.DropTable(
                name: "APPLICATION");

            migrationBuilder.DropIndex(
                name: "IX_REQUEST_LICENSE_ApplicationId",
                table: "REQUEST_LICENSE");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "REQUEST_LICENSE");
        }
    }
}
