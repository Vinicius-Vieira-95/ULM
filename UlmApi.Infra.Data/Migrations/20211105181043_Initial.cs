using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace UlmApi.Infra.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "USER",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "REQUEST_LICENSE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    LicenseType = table.Column<string>(type: "text", nullable: false),
                    JustificationForDeny = table.Column<string>(type: "text", nullable: true),
                    RequesterId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REQUEST_LICENSE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REQUEST_LICENSE_USER_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LICENSE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Label = table.Column<string>(type: "text", nullable: true),
                    ProjectName = table.Column<string>(type: "text", nullable: false),
                    LicenseType = table.Column<string>(type: "text", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AquisitionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Provider = table.Column<string>(type: "text", nullable: false),
                    Justification = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    OwnerId = table.Column<string>(type: "text", nullable: true),
                    RequestId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LICENSE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LICENSE_REQUEST_LICENSE_RequestId",
                        column: x => x.RequestId,
                        principalTable: "REQUEST_LICENSE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LICENSE_USER_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LICENSE_OwnerId",
                table: "LICENSE",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_LICENSE_RequestId",
                table: "LICENSE",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_REQUEST_LICENSE_RequesterId",
                table: "REQUEST_LICENSE",
                column: "RequesterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LICENSE");

            migrationBuilder.DropTable(
                name: "REQUEST_LICENSE");

            migrationBuilder.DropTable(
                name: "USER");
        }
    }
}
