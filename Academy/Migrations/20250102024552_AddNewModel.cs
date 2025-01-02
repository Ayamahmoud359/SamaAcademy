using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrations
{
    /// <inheritdoc />
    public partial class AddNewModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllCompetitionTeamEvaluations",
                columns: table => new
                {
                    AllCompetitionTeamEvaluationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EvaluationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EvaluationImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CompetitionTeamId = table.Column<int>(type: "int", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllCompetitionTeamEvaluations", x => x.AllCompetitionTeamEvaluationId);
                    table.ForeignKey(
                        name: "FK_AllCompetitionTeamEvaluations_CompetitionTeam_CompetitionTeamId",
                        column: x => x.CompetitionTeamId,
                        principalTable: "CompetitionTeam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AllCompetitionTeamEvaluations_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "TrainerId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllCompetitionTeamEvaluations_CompetitionTeamId",
                table: "AllCompetitionTeamEvaluations",
                column: "CompetitionTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_AllCompetitionTeamEvaluations_TrainerId",
                table: "AllCompetitionTeamEvaluations",
                column: "TrainerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllCompetitionTeamEvaluations");
        }
    }
}
