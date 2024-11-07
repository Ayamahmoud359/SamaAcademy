using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abscesses_Children_TraineeId",
                table: "Abscesses");

            migrationBuilder.DropForeignKey(
                name: "FK_ChampionChildren_Champions_ChampionId",
                table: "ChampionChildren");

            migrationBuilder.DropForeignKey(
                name: "FK_ChampionChildren_Children_TraineeId",
                table: "ChampionChildren");

            migrationBuilder.DropForeignKey(
                name: "FK_Children_Parents_ParentId",
                table: "Children");

            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyChildScores_Children_TraineeId",
                table: "MonthlyChildScores");

            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyChildScores_Subscriptions_SubscriptionId",
                table: "MonthlyChildScores");

            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyChildScores_Trainers_TrainerId",
                table: "MonthlyChildScores");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Children_TraineeId",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "SubCategoryTrainer");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MonthlyChildScores",
                table: "MonthlyChildScores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Children",
                table: "Children");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChampionChildren",
                table: "ChampionChildren");

            migrationBuilder.RenameTable(
                name: "MonthlyChildScores",
                newName: "Exams");

            migrationBuilder.RenameTable(
                name: "Children",
                newName: "Trainees");

            migrationBuilder.RenameTable(
                name: "ChampionChildren",
                newName: "TraineeChampions");

            migrationBuilder.RenameColumn(
                name: "SubscriptionDate",
                table: "Subscriptions",
                newName: "StartDate");

            migrationBuilder.RenameIndex(
                name: "IX_MonthlyChildScores_TrainerId",
                table: "Exams",
                newName: "IX_Exams_TrainerId");

            migrationBuilder.RenameIndex(
                name: "IX_MonthlyChildScores_TraineeId",
                table: "Exams",
                newName: "IX_Exams_TraineeId");

            migrationBuilder.RenameIndex(
                name: "IX_MonthlyChildScores_SubscriptionId",
                table: "Exams",
                newName: "IX_Exams_SubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Children_ParentId",
                table: "Trainees",
                newName: "IX_Trainees_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_ChampionChildren_TraineeId",
                table: "TraineeChampions",
                newName: "IX_TraineeChampions_TraineeId");

            migrationBuilder.RenameIndex(
                name: "IX_ChampionChildren_ChampionId",
                table: "TraineeChampions",
                newName: "IX_TraineeChampions_ChampionId");

            migrationBuilder.AddColumn<string>(
                name: "EndDate",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exams",
                table: "Exams",
                column: "ExamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trainees",
                table: "Trainees",
                column: "TraineeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TraineeChampions",
                table: "TraineeChampions",
                column: "TraineeChampionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Abscesses_Trainees_TraineeId",
                table: "Abscesses",
                column: "TraineeId",
                principalTable: "Trainees",
                principalColumn: "TraineeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Subscriptions_SubscriptionId",
                table: "Exams",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "SubscriptionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Trainees_TraineeId",
                table: "Exams",
                column: "TraineeId",
                principalTable: "Trainees",
                principalColumn: "TraineeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Trainers_TrainerId",
                table: "Exams",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Trainees_TraineeId",
                table: "Subscriptions",
                column: "TraineeId",
                principalTable: "Trainees",
                principalColumn: "TraineeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TraineeChampions_Champions_ChampionId",
                table: "TraineeChampions",
                column: "ChampionId",
                principalTable: "Champions",
                principalColumn: "ChampionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TraineeChampions_Trainees_TraineeId",
                table: "TraineeChampions",
                column: "TraineeId",
                principalTable: "Trainees",
                principalColumn: "TraineeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_Parents_ParentId",
                table: "Trainees",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "ParentId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abscesses_Trainees_TraineeId",
                table: "Abscesses");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Subscriptions_SubscriptionId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Trainees_TraineeId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Trainers_TrainerId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Trainees_TraineeId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TraineeChampions_Champions_ChampionId",
                table: "TraineeChampions");

            migrationBuilder.DropForeignKey(
                name: "FK_TraineeChampions_Trainees_TraineeId",
                table: "TraineeChampions");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_Parents_ParentId",
                table: "Trainees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trainees",
                table: "Trainees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TraineeChampions",
                table: "TraineeChampions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exams",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Subscriptions");

            migrationBuilder.RenameTable(
                name: "Trainees",
                newName: "Children");

            migrationBuilder.RenameTable(
                name: "TraineeChampions",
                newName: "ChampionChildren");

            migrationBuilder.RenameTable(
                name: "Exams",
                newName: "MonthlyChildScores");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Subscriptions",
                newName: "SubscriptionDate");

            migrationBuilder.RenameIndex(
                name: "IX_Trainees_ParentId",
                table: "Children",
                newName: "IX_Children_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_TraineeChampions_TraineeId",
                table: "ChampionChildren",
                newName: "IX_ChampionChildren_TraineeId");

            migrationBuilder.RenameIndex(
                name: "IX_TraineeChampions_ChampionId",
                table: "ChampionChildren",
                newName: "IX_ChampionChildren_ChampionId");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_TrainerId",
                table: "MonthlyChildScores",
                newName: "IX_MonthlyChildScores_TrainerId");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_TraineeId",
                table: "MonthlyChildScores",
                newName: "IX_MonthlyChildScores_TraineeId");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_SubscriptionId",
                table: "MonthlyChildScores",
                newName: "IX_MonthlyChildScores_SubscriptionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Children",
                table: "Children",
                column: "TraineeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChampionChildren",
                table: "ChampionChildren",
                column: "TraineeChampionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonthlyChildScores",
                table: "MonthlyChildScores",
                column: "ExamId");

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    SubCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    SubCategoryDescriptionAR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubCategoryDescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubCategoryNameAR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubCategoryNameEN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.SubCategoryId);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubCategoryTrainer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubCategoryId = table.Column<int>(type: "int", nullable: true),
                    TrainerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategoryTrainer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategoryTrainer_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "SubCategoryId");
                    table.ForeignKey(
                        name: "FK_SubCategoryTrainer_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "TrainerId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategoryTrainer_SubCategoryId",
                table: "SubCategoryTrainer",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategoryTrainer_TrainerId",
                table: "SubCategoryTrainer",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Abscesses_Children_TraineeId",
                table: "Abscesses",
                column: "TraineeId",
                principalTable: "Children",
                principalColumn: "TraineeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChampionChildren_Champions_ChampionId",
                table: "ChampionChildren",
                column: "ChampionId",
                principalTable: "Champions",
                principalColumn: "ChampionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChampionChildren_Children_TraineeId",
                table: "ChampionChildren",
                column: "TraineeId",
                principalTable: "Children",
                principalColumn: "TraineeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Children_Parents_ParentId",
                table: "Children",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "ParentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyChildScores_Children_TraineeId",
                table: "MonthlyChildScores",
                column: "TraineeId",
                principalTable: "Children",
                principalColumn: "TraineeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyChildScores_Subscriptions_SubscriptionId",
                table: "MonthlyChildScores",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "SubscriptionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyChildScores_Trainers_TrainerId",
                table: "MonthlyChildScores",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Children_TraineeId",
                table: "Subscriptions",
                column: "TraineeId",
                principalTable: "Children",
                principalColumn: "TraineeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
