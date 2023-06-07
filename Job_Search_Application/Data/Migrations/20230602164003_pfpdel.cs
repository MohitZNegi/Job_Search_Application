using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job_Search_Application.Data.Migrations
{
    public partial class pfpdel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Employee");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
