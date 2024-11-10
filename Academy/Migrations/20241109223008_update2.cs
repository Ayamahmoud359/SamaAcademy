using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrations
{
    /// <inheritdoc />
    public partial class update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abscenses_Trainers_TrainerId",
                table: "Abscenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Trainers_TrainerId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Parents_Branches_BranchId",
                table: "Parents");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Trainers_TrainerId",
                table: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "TrainerId",
                table: "Subscriptions",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_TrainerId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_DepartmentId");

            migrationBuilder.RenameColumn(
                name: "AbsentDate",
                table: "Abscenses",
                newName: "AbsenceDate");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Trainees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Trainees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResidencyNumber",
                table: "Trainees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TraineeAddress",
                table: "Trainees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TraineeEmail",
                table: "Trainees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TraineePhone",
                table: "Trainees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Parents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TrainerId",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "Exams",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Abscenses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "TrainerId",
                table: "Abscenses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trainees_BranchId",
                table: "Trainees",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Abscenses_Trainers_TrainerId",
                table: "Abscenses",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "TrainerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Trainers_TrainerId",
                table: "Exams",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "TrainerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parents_Branches_BranchId",
                table: "Parents",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Departments_DepartmentId",
                table: "Subscriptions",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_Branches_BranchId",
                table: "Trainees",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abscenses_Trainers_TrainerId",
                table: "Abscenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Trainers_TrainerId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Parents_Branches_BranchId",
                table: "Parents");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Departments_DepartmentId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_Branches_BranchId",
                table: "Trainees");

            migrationBuilder.DropIndex(
                name: "IX_Trainees_BranchId",
                table: "Trainees");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Trainees");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Trainees");

            migrationBuilder.DropColumn(
                name: "ResidencyNumber",
                table: "Trainees");

            migrationBuilder.DropColumn(
                name: "TraineeAddress",
                table: "Trainees");

            migrationBuilder.DropColumn(
                name: "TraineeEmail",
                table: "Trainees");

            migrationBuilder.DropColumn(
                name: "TraineePhone",
                table: "Trainees");

            migrationBuilder.DropColumn(
                name: "Review",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Subscriptions",
                newName: "TrainerId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_DepartmentId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_TrainerId");

            migrationBuilder.RenameColumn(
                name: "AbsenceDate",
                table: "Abscenses",
                newName: "AbsentDate");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Parents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrainerId",
                table: "Exams",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Abscenses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrainerId",
                table: "Abscenses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Abscenses_Trainers_TrainerId",
                table: "Abscenses",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Trainers_TrainerId",
                table: "Exams",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parents_Branches_BranchId",
                table: "Parents",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Trainers_TrainerId",
                table: "Subscriptions",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "TrainerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
