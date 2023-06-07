using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job_Search_Application.Data.Migrations
{
    public partial class pfpurldrop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resume",
                table: "Employee");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          
        }
    }
}
