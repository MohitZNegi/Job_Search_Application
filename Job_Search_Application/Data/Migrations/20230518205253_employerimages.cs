using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job_Search_Application.Data.Migrations
{
    public partial class employerimages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company_Logo",
                table: "Employer");
            migrationBuilder.DropColumn(
                name: "Company_Banner",
                table: "Employer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
