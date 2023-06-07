using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job_Search_Application.Data.Migrations
{
    public partial class employerimageadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<IFormFile>(
                name: "Company_Banner",
                table: "Employer",
                defaultValue: "Image");

            migrationBuilder.AddColumn<IFormFile>(
                name: "Company_Logo",
                table: "Employer",
                defaultValue: "Image");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
