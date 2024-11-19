using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrations
{
    /// <inheritdoc />
    public partial class update10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Champions_Departments_DepartmentId",
                table: "Champions");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_CompetitionTeam_CompetitionTeamId",
                table: "Trainees");

            migrationBuilder.DropTable(
                name: "TraineeChampions");

            migrationBuilder.DropIndex(
                name: "IX_Trainees_CompetitionTeamId",
                table: "Trainees");

            migrationBuilder.DropIndex(
                name: "IX_Champions_DepartmentId",
                table: "Champions");

            migrationBuilder.DropColumn(
                name: "CompetitionTeamId",
                table: "Trainees");

            migrationBuilder.DropColumn(
                name: "ChampionScore",
                table: "Champions");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Champions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Champions");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Champions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TraineeCompetitionTeams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TraineeId = table.Column<int>(type: "int", nullable: false),
                    CompetitionTeamId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraineeCompetitionTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TraineeCompetitionTeams_CompetitionTeam_CompetitionTeamId",
                        column: x => x.CompetitionTeamId,
                        principalTable: "CompetitionTeam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TraineeCompetitionTeams_Trainees_TraineeId",
                        column: x => x.TraineeId,
                        principalTable: "Trainees",
                        principalColumn: "TraineeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TraineeCompetitionTeams_CompetitionTeamId",
                table: "TraineeCompetitionTeams",
                column: "CompetitionTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TraineeCompetitionTeams_TraineeId",
                table: "TraineeCompetitionTeams",
                column: "TraineeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TraineeCompetitionTeams");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Champions");

            migrationBuilder.AddColumn<int>(
                name: "CompetitionTeamId",
                table: "Trainees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChampionScore",
                table: "Champions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Champions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Champions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TraineeChampions",
                columns: table => new
                {
                    TraineeChampionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChampionId = table.Column<int>(type: "int", nullable: false),
                    TraineeId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraineeChampions", x => x.TraineeChampionId);
                    table.ForeignKey(
                        name: "FK_TraineeChampions_Champions_ChampionId",
                        column: x => x.ChampionId,
                        principalTable: "Champions",
                        principalColumn: "ChampionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TraineeChampions_Trainees_TraineeId",
                        column: x => x.TraineeId,
                        principalTable: "Trainees",
                        principalColumn: "TraineeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trainees_CompetitionTeamId",
                table: "Trainees",
                column: "CompetitionTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Champions_DepartmentId",
                table: "Champions",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TraineeChampions_ChampionId",
                table: "TraineeChampions",
                column: "ChampionId");

            migrationBuilder.CreateIndex(
                name: "IX_TraineeChampions_TraineeId",
                table: "TraineeChampions",
                column: "TraineeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Champions_Departments_DepartmentId",
                table: "Champions",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_CompetitionTeam_CompetitionTeamId",
                table: "Trainees",
                column: "CompetitionTeamId",
                principalTable: "CompetitionTeam",
                principalColumn: "Id");
        }
    }
}
