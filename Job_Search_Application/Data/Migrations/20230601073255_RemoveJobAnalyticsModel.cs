using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Job_Search_Application.Data.Migrations
{
    public partial class RemoveJobAnalyticsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Job_Analytics");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Job_Analytics",
                columns: table => new
                {
                    JobAnalysis_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Jobs_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Applies = table.Column<int>(type: "int", nullable: false),
                    HiredCandidates = table.Column<int>(type: "int", nullable: false),
                    Id_Job = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InterviewedCandidates = table.Column<int>(type: "int", nullable: false),
                    ReviewedCandidates = table.Column<int>(type: "int", nullable: false),
                    SelectedCandidates = table.Column<int>(type: "int", nullable: false),
                    Views = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job_Analytics", x => x.JobAnalysis_Id);
                    table.ForeignKey(
                        name: "FK_Job_Analytics_Employer_EmployerId",
                        column: x => x.EmployerId,
                        principalTable: "Employer",
                        principalColumn: "Employer_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Job_Analytics_Jobs_Jobs_Id",
                        column: x => x.Jobs_Id,
                        principalTable: "Jobs",
                        principalColumn: "Jobs_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Job_Analytics_EmployerId",
                table: "Job_Analytics",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_Job_Analytics_Jobs_Id",
                table: "Job_Analytics",
                column: "Jobs_Id");
        }
    }
}
