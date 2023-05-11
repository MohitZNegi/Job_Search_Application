using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job_Search_Application.Data.Migrations
{
    public partial class role : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Roles_RoleDataId",
                schema: "security",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_RoleDataId",
                schema: "security",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                schema: "security",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "RoleDataId",
                schema: "security",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "User_role",
                schema: "security",
                table: "Roles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                schema: "security",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoleDataId",
                schema: "security",
                table: "Roles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "User_role",
                schema: "security",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleDataId",
                schema: "security",
                table: "Roles",
                column: "RoleDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Roles_RoleDataId",
                schema: "security",
                table: "Roles",
                column: "RoleDataId",
                principalSchema: "security",
                principalTable: "Roles",
                principalColumn: "Id");
        }
    }
}
