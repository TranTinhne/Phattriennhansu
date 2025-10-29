using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseCatalog",
                columns: table => new
                {
                    CatalogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DurationHours = table.Column<decimal>(type: "decimal(4,1)", nullable: true),
                    Provider = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCatalog", x => x.CatalogID);
                });

            migrationBuilder.CreateTable(
                name: "SurveyPeriods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyPeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPrograms",
                columns: table => new
                {
                    ProgramID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProgramName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<short>(type: "smallint", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPrograms", x => x.ProgramID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSurveyResponses",
                columns: table => new
                {
                    ResponseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    CatalogID = table.Column<int>(type: "int", nullable: false),
                    SurveyPeriodId = table.Column<int>(type: "int", nullable: false),
                    LevelUpdateKnowledge = table.Column<bool>(type: "bit", nullable: false),
                    LevelEnhanceKnowledge = table.Column<bool>(type: "bit", nullable: false),
                    LevelNecessary = table.Column<bool>(type: "bit", nullable: false),
                    LevelVeryNecessary = table.Column<bool>(type: "bit", nullable: false),
                    IsPriority = table.Column<bool>(type: "bit", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSurveyResponses", x => x.ResponseID);
                    table.ForeignKey(
                        name: "FK_EmployeeSurveyResponses_CourseCatalog_CatalogID",
                        column: x => x.CatalogID,
                        principalTable: "CourseCatalog",
                        principalColumn: "CatalogID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeSurveyResponses_SurveyPeriods_SurveyPeriodId",
                        column: x => x.SurveyPeriodId,
                        principalTable: "SurveyPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingCourses",
                columns: table => new
                {
                    CourseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramID = table.Column<int>(type: "int", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Instructor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Duration = table.Column<decimal>(type: "decimal(5,1)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaxAttendees = table.Column<int>(type: "int", nullable: false),
                    RequiresApproval = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<short>(type: "smallint", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingCourses", x => x.CourseID);
                    table.ForeignKey(
                        name: "FK_TrainingCourses_TrainingPrograms_ProgramID",
                        column: x => x.ProgramID,
                        principalTable: "TrainingPrograms",
                        principalColumn: "ProgramID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTrainingRegistrations",
                columns: table => new
                {
                    RegistrationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    CourseID = table.Column<int>(type: "int", nullable: false),
                    ProgramID = table.Column<int>(type: "int", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ApproverId = table.Column<int>(type: "int", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<short>(type: "smallint", nullable: false),
                    Score = table.Column<float>(type: "real", nullable: true),
                    Certificate = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTrainingRegistrations", x => x.RegistrationID);
                    table.ForeignKey(
                        name: "FK_EmployeeTrainingRegistrations_TrainingCourses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "TrainingCourses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeTrainingRegistrations_TrainingPrograms_ProgramID",
                        column: x => x.ProgramID,
                        principalTable: "TrainingPrograms",
                        principalColumn: "ProgramID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseCatalog_CourseCode",
                table: "CourseCatalog",
                column: "CourseCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSurveyResponses_CatalogID",
                table: "EmployeeSurveyResponses",
                column: "CatalogID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSurveyResponses_SurveyPeriodId",
                table: "EmployeeSurveyResponses",
                column: "SurveyPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_Employee_Survey_Item",
                table: "EmployeeSurveyResponses",
                columns: new[] { "EmployeeID", "CatalogID", "SurveyPeriodId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTrainingRegistrations_CourseID",
                table: "EmployeeTrainingRegistrations",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTrainingRegistrations_ProgramID",
                table: "EmployeeTrainingRegistrations",
                column: "ProgramID");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_Employee_Course_Registration",
                table: "EmployeeTrainingRegistrations",
                columns: new[] { "EmployeeID", "CourseID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCourses_ProgramID",
                table: "TrainingCourses",
                column: "ProgramID");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPrograms_ProgramCode",
                table: "TrainingPrograms",
                column: "ProgramCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeSurveyResponses");

            migrationBuilder.DropTable(
                name: "EmployeeTrainingRegistrations");

            migrationBuilder.DropTable(
                name: "CourseCatalog");

            migrationBuilder.DropTable(
                name: "SurveyPeriods");

            migrationBuilder.DropTable(
                name: "TrainingCourses");

            migrationBuilder.DropTable(
                name: "TrainingPrograms");
        }
    }
}
