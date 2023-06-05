using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job_Search_Application.Data.Migrations
{
    public partial class JobsModelUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Applies",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "Jobs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishDate",
                table: "Jobs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Jobs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Jobs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishDate",
                table: "Jobs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Applies",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
