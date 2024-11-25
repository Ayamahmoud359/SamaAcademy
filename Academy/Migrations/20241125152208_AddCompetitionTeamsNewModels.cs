using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrations
{
    /// <inheritdoc />
    public partial class AddCompetitionTeamsNewModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompetitionTeamAbsences",
                columns: table => new
                {
                    CompetitionTeamAbsenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAbsent = table.Column<bool>(type: "bit", nullable: false),
                    AbsenceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TraineeCompetitionTeamId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionTeamAbsences", x => x.CompetitionTeamAbsenceId);
                    table.ForeignKey(
                        name: "FK_CompetitionTeamAbsences_TraineeCompetitionTeams_TraineeCompetitionTeamId",
                        column: x => x.TraineeCompetitionTeamId,
                        principalTable: "TraineeCompetitionTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionTeamAbsences_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "TrainerId");
                });

            migrationBuilder.CreateTable(
                name: "CompetitionTeamEvaluations",
                columns: table => new
                {
                    CompetitionTeamEvaluationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: true),
                    Review = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TraineeCompetitionTeamId = table.Column<int>(type: "int", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionTeamEvaluations", x => x.CompetitionTeamEvaluationId);
                    table.ForeignKey(
                        name: "FK_CompetitionTeamEvaluations_TraineeCompetitionTeams_TraineeCompetitionTeamId",
                        column: x => x.TraineeCompetitionTeamId,
                        principalTable: "TraineeCompetitionTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionTeamEvaluations_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "TrainerId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionTeamAbsences_TraineeCompetitionTeamId",
                table: "CompetitionTeamAbsences",
                column: "TraineeCompetitionTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionTeamAbsences_TrainerId",
                table: "CompetitionTeamAbsences",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionTeamEvaluations_TraineeCompetitionTeamId",
                table: "CompetitionTeamEvaluations",
                column: "TraineeCompetitionTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionTeamEvaluations_TrainerId",
                table: "CompetitionTeamEvaluations",
                column: "TrainerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetitionTeamAbsences");

            migrationBuilder.DropTable(
                name: "CompetitionTeamEvaluations");
        }
    }
}
