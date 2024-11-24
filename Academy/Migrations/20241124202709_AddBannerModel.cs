using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrations
{
    /// <inheritdoc />
    public partial class AddBannerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionTeam_Trainers_TrainerId",
                table: "CompetitionTeam");

            migrationBuilder.AlterColumn<int>(
                name: "TrainerId",
                table: "CompetitionTeam",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionTeam_Trainers_TrainerId",
                table: "CompetitionTeam",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "TrainerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionTeam_Trainers_TrainerId",
                table: "CompetitionTeam");

            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.AlterColumn<int>(
                name: "TrainerId",
                table: "CompetitionTeam",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionTeam_Trainers_TrainerId",
                table: "CompetitionTeam",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "TrainerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
