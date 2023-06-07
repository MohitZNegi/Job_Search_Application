using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job_Search_Application.Data.Migrations
{
<<<<<<<< HEAD:Job_Search_Application/Data/Migrations/20230602163304_pfp.cs
    public partial class pfp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Employee");
========
    public partial class Profileimageadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<IFormFile>(
                name: "ProfileImage",
                table: "Employee",
                defaultValue: "Image");
>>>>>>>> 8f6b19596f62271f1db1078b0cf0334468d2944b:Job_Search_Application/Data/Migrations/20230604234833_Profileimageadded.cs
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
