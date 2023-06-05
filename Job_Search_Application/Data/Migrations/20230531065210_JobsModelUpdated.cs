using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job_Search_Application.Data.Migrations
{
    public partial class JobsModelUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Applies",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivationDate",
                table: "Jobs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Jobs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                table: "Jobs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Applies",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "DeactivationDate",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "Jobs");
        }
    }
}
