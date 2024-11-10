using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrations
{
    /// <inheritdoc />
    public partial class updatetables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_Branches_BranchId",
                table: "Trainees");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainers_Departments_DepartmentId",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Trainers_DepartmentId",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Trainees_BranchId",
                table: "Trainees");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Trainees");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Trainers",
                newName: "Branch");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TraineeChampions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CategoryTrainers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_BranchId",
                table: "Subscriptions",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Branches_BranchId",
                table: "Subscriptions",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Branches_BranchId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_BranchId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TraineeChampions");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CategoryTrainers");

            migrationBuilder.RenameColumn(
                name: "Branch",
                table: "Trainers",
                newName: "DepartmentId");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Trainees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_DepartmentId",
                table: "Trainers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainees_BranchId",
                table: "Trainees",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_Branches_BranchId",
                table: "Trainees",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainers_Departments_DepartmentId",
                table: "Trainers",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
