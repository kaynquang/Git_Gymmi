using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymmi.Migrations
{
    /// <inheritdoc />
    public partial class AddDemoTrainer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert demo trainer users
            migrationBuilder.Sql(@"
                INSERT INTO Users (HoTen, TenDangNhap, Sdt, Email, Password, ID_Role, CreatedAt, UpdatedAt)
                VALUES 
                ('Nguyễn Văn Minh', 'trainer1', '0987654321', 'trainer1@gymmi.com', 'trainer123', 4, GETDATE(), GETDATE()),
                ('Trần Thị Lan', 'trainer2', '0976543210', 'trainer2@gymmi.com', 'trainer123', 4, GETDATE(), GETDATE()),
                ('Lê Hoàng Nam', 'trainer3', '0965432109', 'trainer3@gymmi.com', 'trainer123', 4, GETDATE(), GETDATE())
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove demo trainer users
            migrationBuilder.Sql(@"
                DELETE FROM Users WHERE TenDangNhap IN ('trainer1', 'trainer2', 'trainer3')
            ");
        }
    }
}
