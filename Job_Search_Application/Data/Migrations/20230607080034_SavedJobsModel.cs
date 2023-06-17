using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job_Search_Application.Data.Migrations
{
    public partial class SavedJobsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SavedJobs",
                columns: table => new
                {
                    SavedJobs_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JobId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedJobs", x => x.SavedJobs_Id);
                    table.ForeignKey(
                        name: "FK_SavedJobs_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Employee_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedJobs_Employer_EmployerId",
                        column: x => x.EmployerId,
                        principalTable: "Employer",
                        principalColumn: "Employer_Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SavedJobs_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Jobs_Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavedJobs_EmployeeId",
                table: "SavedJobs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedJobs_EmployerId",
                table: "SavedJobs",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedJobs_JobId",
                table: "SavedJobs",
                column: "JobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedJobs");
        }
    }
}
