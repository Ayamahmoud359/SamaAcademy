using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrations
{
    /// <inheritdoc />
    public partial class update3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abscenses_Trainees_TraineeId",
                table: "Abscenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Trainees_TraineeId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Parents_Branches_BranchId",
                table: "Parents");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Branches_BranchId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Departments_DepartmentId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainers_Branches_BranchId",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Trainers_BranchId",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_BranchId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_DepartmentId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Parents_BranchId",
                table: "Parents");

            migrationBuilder.DropIndex(
                name: "IX_Exams_TraineeId",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Abscenses_TraineeId",
                table: "Abscenses");

            migrationBuilder.DropColumn(
                name: "Branch",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "TraineeId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "TraineeId",
                table: "Abscenses");

            migrationBuilder.RenameColumn(
                name: "BranchId",
                table: "Trainers",
                newName: "CurrentDepartment");

            migrationBuilder.AddColumn<int>(
                name: "CurrentBranch",
                table: "Trainers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Branch",
                table: "Subscriptions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Department",
                table: "Subscriptions",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentBranch",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "Branch",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "CurrentDepartment",
                table: "Trainers",
                newName: "BranchId");

            migrationBuilder.AddColumn<int>(
                name: "Branch",
                table: "Trainers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Parents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TraineeId",
                table: "Exams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TraineeId",
                table: "Abscenses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_BranchId",
                table: "Trainers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_BranchId",
                table: "Subscriptions",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_DepartmentId",
                table: "Subscriptions",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_BranchId",
                table: "Parents",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_TraineeId",
                table: "Exams",
                column: "TraineeId");

            migrationBuilder.CreateIndex(
                name: "IX_Abscenses_TraineeId",
                table: "Abscenses",
                column: "TraineeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Abscenses_Trainees_TraineeId",
                table: "Abscenses",
                column: "TraineeId",
                principalTable: "Trainees",
                principalColumn: "TraineeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Trainees_TraineeId",
                table: "Exams",
                column: "TraineeId",
                principalTable: "Trainees",
                principalColumn: "TraineeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parents_Branches_BranchId",
                table: "Parents",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Branches_BranchId",
                table: "Subscriptions",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Departments_DepartmentId",
                table: "Subscriptions",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainers_Branches_BranchId",
                table: "Trainers",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId");
        }
    }
}
