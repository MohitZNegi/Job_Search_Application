using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job_Search_Application.Data.Migrations
{
    public partial class Profileimageadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<IFormFile>(
                name: "ProfileImage",
                table: "Employee",
                defaultValue: "Image");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
