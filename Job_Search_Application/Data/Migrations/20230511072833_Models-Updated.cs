using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job_Search_Application.Data.Migrations
{
    public partial class ModelsUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.DropColumn(
                name: "First_name",
                schema: "security",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Last_name",
                schema: "security",
                table: "Users");

            migrationBuilder.DropTable(
               name: "Employers");

            migrationBuilder.DropTable(
               name: "Employees");




        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company_Banner",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "Company_CEO",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "Company_Industry",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "Company_URL",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "First_name",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Last_name",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Resume",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "First_name",
                schema: "security",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Last_name",
                schema: "security",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Company_Name",
                table: "Employers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Employees",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Employee_Id",
                table: "Employees",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("SqlServer:Identity", "1, 1");
        }
    }
}
