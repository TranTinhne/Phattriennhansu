using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SurveyPeriods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Keyword = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyPeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingProviders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsInternal = table.Column<bool>(type: "bit", nullable: false),
                    Keyword = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseCatalog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DurationHours = table.Column<decimal>(type: "decimal(4,1)", nullable: true),
                    ProviderID = table.Column<int>(type: "int", maxLength: 255, nullable: true),
                    Keyword = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCatalog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseCatalog_TrainingProviders_ProviderID",
                        column: x => x.ProviderID,
                        principalTable: "TrainingProviders",
                        principalColumn: "Id");
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
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSurveyResponses", x => x.ResponseID);
                    table.ForeignKey(
                        name: "FK_EmployeeSurveyResponses_CourseCatalog_CatalogID",
                        column: x => x.CatalogID,
                        principalTable: "CourseCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeSurveyResponses_SurveyPeriods_SurveyPeriodId",
                        column: x => x.SurveyPeriodId,
                        principalTable: "SurveyPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseCatalog_CourseCode",
                table: "CourseCatalog",
                column: "CourseCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseCatalog_ProviderID",
                table: "CourseCatalog",
                column: "ProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSurveyResponses_CatalogID",
                table: "EmployeeSurveyResponses",
                column: "CatalogID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSurveyResponses_EmployeeID_CatalogID_SurveyPeriodId",
                table: "EmployeeSurveyResponses",
                columns: new[] { "EmployeeID", "CatalogID", "SurveyPeriodId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSurveyResponses_SurveyPeriodId",
                table: "EmployeeSurveyResponses",
                column: "SurveyPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingProviders_ProviderCode",
                table: "TrainingProviders",
                column: "ProviderCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeSurveyResponses");

            migrationBuilder.DropTable(
                name: "CourseCatalog");

            migrationBuilder.DropTable(
                name: "SurveyPeriods");

            migrationBuilder.DropTable(
                name: "TrainingProviders");
        }
    }
}
