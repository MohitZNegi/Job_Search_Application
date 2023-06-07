using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job_Search_Application.Data.Migrations
{
    public partial class resumeadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<IFormFile>(
                name: "Resume",
                table: "Employee",
                defaultValue: "Resume");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
