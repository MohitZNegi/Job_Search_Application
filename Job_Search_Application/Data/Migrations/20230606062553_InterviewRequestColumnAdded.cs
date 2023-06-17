using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job_Search_Application.Data.Migrations
{
    public partial class InterviewRequestColumnAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InterviewRequest_Date",
                table: "Job_Request",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InterviewRequest_Status",
                table: "Job_Request",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterviewRequest_Date",
                table: "Job_Request");

            migrationBuilder.DropColumn(
                name: "InterviewRequest_Status",
                table: "Job_Request");
        }
    }
}
