using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job_Search_Application.Data.Migrations
{
    public partial class JobsModelAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Jobs_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PublisherId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Job_Details = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Job_Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Job_Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Job_Schedule = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Classification = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Jobs_Id);
                    table.ForeignKey(
                        name: "FK_Jobs_Employer_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Employer",
                        principalColumn: "Employer_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_PublisherId",
                table: "Jobs",
                column: "PublisherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jobs");
        }
    }
}
