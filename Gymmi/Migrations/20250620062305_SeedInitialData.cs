using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gymmi.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CaLamViecs",
                columns: new[] { "ID_Ca", "MoTa", "TenCa" },
                values: new object[,]
                {
                    { 1, "6:00 - 14:00", "Ca sáng" },
                    { 2, "14:00 - 22:00", "Ca chiều" },
                    { 3, "18:00 - 02:00", "Ca tối" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "ID_Role", "MoTa", "TenRole" },
                values: new object[,]
                {
                    { 1, "Quản trị viên hệ thống", "Admin" },
                    { 2, "Nhân viên phòng tập", "Nhân viên" },
                    { 3, "Thành viên phòng tập", "Hội viên" },
                    { 4, "Huấn luyện viên cá nhân", "Huấn luyện viên" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CaLamViecs",
                keyColumn: "ID_Ca",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CaLamViecs",
                keyColumn: "ID_Ca",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CaLamViecs",
                keyColumn: "ID_Ca",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID_Role",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID_Role",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID_Role",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID_Role",
                keyValue: 4);
        }
    }
}
