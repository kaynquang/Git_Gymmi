using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymmi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExistingHoiVienTrangThai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update existing records to have default status
            migrationBuilder.Sql("UPDATE HoiViens SET TrangThai = 'Hoạt động' WHERE TrangThai = ''");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
