using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UlmApi.Infra.Data.Migrations
{
    public partial class AddCreationDateInApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_REQUEST_LICENSE_APPLICATION_ApplicationId",
                table: "REQUEST_LICENSE");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationId",
                table: "REQUEST_LICENSE",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "APPLICATION",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "APPLICATION");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationId",
                table: "REQUEST_LICENSE",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_REQUEST_LICENSE_APPLICATION_ApplicationId",
                table: "REQUEST_LICENSE",
                column: "ApplicationId",
                principalTable: "APPLICATION",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
