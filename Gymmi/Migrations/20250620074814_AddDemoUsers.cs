using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymmi.Migrations
{
    /// <inheritdoc />
    public partial class AddDemoUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert demo users
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID_User", "HoTen", "TenDangNhap", "Sdt", "Email", "Password", "ID_Role", "CreatedAt", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Admin Gym", "admin", "0901234567", "admin@gymmi.com", "admin123", 1, DateTime.Now, DateTime.Now },
                    { 2, "Nguyễn Văn Member", "member", "0902345678", "member@gymmi.com", "member123", 3, DateTime.Now, DateTime.Now },
                    { 3, "Trần Thị Staff", "staff", "0903456789", "staff@gymmi.com", "staff123", 2, DateTime.Now, DateTime.Now },
                    { 4, "Lê Văn Trainer", "trainer", "0904567890", "trainer@gymmi.com", "trainer123", 4, DateTime.Now, DateTime.Now }
                });

            // Insert demo member info for the member user
            migrationBuilder.InsertData(
                table: "HoiViens",
                columns: new[] { "ID_HoiVien", "ID_User", "HoTen", "NgaySinh", "GioiTinh", "Sdt", "Email", "DiaChi", "NgayThamGia" },
                values: new object[] { 1, 2, "Nguyễn Văn Member", new DateTime(1990, 5, 15), "Nam", "0902345678", "member@gymmi.com", "123 Nguyễn Văn Cừ, Q.5, TP.HCM", DateTime.Now });

            // Insert demo packages
            migrationBuilder.InsertData(
                table: "GoiTaps",
                columns: new[] { "ID_GoiTap", "TenGoi", "GiaTien", "SoNgay", "LoaiGoiTap", "MoTa", "NgayBatDau", "NgayKetThuc" },
                values: new object[,]
                {
                    { 1, "Gói Basic", 500000, 30, "Basic", "Gói tập cơ bản với đầy đủ thiết bị", DateTime.Now, DateTime.Now.AddMonths(12) },
                    { 2, "Gói Premium", 800000, 30, "Premium", "Gói tập cao cấp với huấn luyện viên", DateTime.Now, DateTime.Now.AddMonths(12) },
                    { 3, "Gói VIP", 1200000, 30, "VIP", "Gói tập VIP với dịch vụ spa và đỗ xe miễn phí", DateTime.Now, DateTime.Now.AddMonths(12) }
                });

            // Insert demo membership card for the member
            migrationBuilder.InsertData(
                table: "TheHoiViens",
                columns: new[] { "ID_TheHoiVien", "ID_HoiVien", "ID_GoiTap", "ID_User", "LoaiTheHoiVien", "NgayBatDau", "NgayHetHan", "TrangThai" },
                values: new object[] { 1, 1, 2, 2, "Premium", DateTime.Now, DateTime.Now.AddDays(30), "Đang hoạt động" });

            // Insert demo rooms
            migrationBuilder.InsertData(
                table: "PhongTaps",
                columns: new[] { "ID_PhongTap", "TenPhong", "DiaChi", "SucChua", "ViTri", "TinhTrang", "ID_User" },
                values: new object[,]
                {
                    { 1, "Phòng Cardio", "Tầng 1", 20, "Khu A", "Hoạt động", 1 },
                    { 2, "Phòng Weight Training", "Tầng 2", 15, "Khu B", "Hoạt động", 1 },
                    { 3, "Phòng Yoga", "Tầng 3", 25, "Khu C", "Hoạt động", 1 }
                });

            // Insert demo equipment
            migrationBuilder.InsertData(
                table: "ThietBis",
                columns: new[] { "ID_ThietBi", "ID_Phong", "TenThietBi", "SoLuong", "XuatXu", "NgayMua", "TinhTrang", "MoTa", "ID_User" },
                values: new object[,]
                {
                    { 1, 1, "Treadmill", 5, "USA", DateTime.Now.AddMonths(-6), "Hoạt động", "Máy chạy bộ chuyên nghiệp", 1 },
                    { 2, 1, "Exercise Bike", 8, "Germany", DateTime.Now.AddMonths(-4), "Hoạt động", "Xe đạp tập thể dục", 1 },
                    { 3, 2, "Dumbbell Set", 10, "China", DateTime.Now.AddMonths(-3), "Hoạt động", "Bộ tạ đơn đa trọng lượng", 1 },
                    { 4, 2, "Barbell", 3, "USA", DateTime.Now.AddMonths(-5), "Hoạt động", "Thanh tạ thẳng Olympic", 1 },
                    { 5, 3, "Yoga Mat", 30, "India", DateTime.Now.AddMonths(-2), "Hoạt động", "Thảm yoga chống trượt", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete demo data in reverse order
            migrationBuilder.DeleteData(table: "ThietBis", keyColumn: "ID_ThietBi", keyValues: new object[] { 1, 2, 3, 4, 5 });
            migrationBuilder.DeleteData(table: "PhongTaps", keyColumn: "ID_PhongTap", keyValues: new object[] { 1, 2, 3 });
            migrationBuilder.DeleteData(table: "TheHoiViens", keyColumn: "ID_TheHoiVien", keyValue: 1);
            migrationBuilder.DeleteData(table: "GoiTaps", keyColumn: "ID_GoiTap", keyValues: new object[] { 1, 2, 3 });
            migrationBuilder.DeleteData(table: "HoiViens", keyColumn: "ID_HoiVien", keyValue: 1);
            migrationBuilder.DeleteData(table: "Users", keyColumn: "ID_User", keyValues: new object[] { 1, 2, 3, 4 });
        }
    }
}
