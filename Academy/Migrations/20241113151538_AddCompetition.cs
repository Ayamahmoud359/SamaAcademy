using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrations
{
    /// <inheritdoc />
    public partial class AddCompetition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompetitionTeamId",
                table: "Trainees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CompetitionDepartment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionDepartment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionTeam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CompetitionDepartmentId = table.Column<int>(type: "int", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetitionTeam_CompetitionDepartment_CompetitionDepartmentId",
                        column: x => x.CompetitionDepartmentId,
                        principalTable: "CompetitionDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompetitionTeam_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "TrainerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trainees_CompetitionTeamId",
                table: "Trainees",
                column: "CompetitionTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionTeam_CompetitionDepartmentId",
                table: "CompetitionTeam",
                column: "CompetitionDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionTeam_TrainerId",
                table: "CompetitionTeam",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_CompetitionTeam_CompetitionTeamId",
                table: "Trainees",
                column: "CompetitionTeamId",
                principalTable: "CompetitionTeam",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_CompetitionTeam_CompetitionTeamId",
                table: "Trainees");

            migrationBuilder.DropTable(
                name: "CompetitionTeam");

            migrationBuilder.DropTable(
                name: "CompetitionDepartment");

            migrationBuilder.DropIndex(
                name: "IX_Trainees_CompetitionTeamId",
                table: "Trainees");

            migrationBuilder.DropColumn(
                name: "CompetitionTeamId",
                table: "Trainees");
        }
    }
}
