using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymmi.Migrations
{
    /// <inheritdoc />
    public partial class AddSlotManagementToPhongTap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ID_TrainerPhuTrach",
                table: "PhongTaps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SlotDaDangKy",
                table: "PhongTaps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SlotToiDa",
                table: "PhongTaps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PhongTaps_ID_TrainerPhuTrach",
                table: "PhongTaps",
                column: "ID_TrainerPhuTrach");

            migrationBuilder.AddForeignKey(
                name: "FK_PhongTaps_Users_ID_TrainerPhuTrach",
                table: "PhongTaps",
                column: "ID_TrainerPhuTrach",
                principalTable: "Users",
                principalColumn: "ID_User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhongTaps_Users_ID_TrainerPhuTrach",
                table: "PhongTaps");

            migrationBuilder.DropIndex(
                name: "IX_PhongTaps_ID_TrainerPhuTrach",
                table: "PhongTaps");

            migrationBuilder.DropColumn(
                name: "ID_TrainerPhuTrach",
                table: "PhongTaps");

            migrationBuilder.DropColumn(
                name: "SlotDaDangKy",
                table: "PhongTaps");

            migrationBuilder.DropColumn(
                name: "SlotToiDa",
                table: "PhongTaps");
        }
    }
}
