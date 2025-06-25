using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymmi.Migrations
{
    /// <inheritdoc />
    public partial class InitialGymSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaLamViecs",
                columns: table => new
                {
                    ID_Ca = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenCa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaLamViecs", x => x.ID_Ca);
                });

            migrationBuilder.CreateTable(
                name: "GoiTaps",
                columns: table => new
                {
                    ID_GoiTap = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenGoi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LoaiGoiTap = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GiaTien = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoNgay = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoiTaps", x => x.ID_GoiTap);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    ID_Role = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenRole = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.ID_Role);
                });

            migrationBuilder.CreateTable(
                name: "BaoCaos",
                columns: table => new
                {
                    ID_BaoCao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoaiBaoCao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ID_User = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaoCaos", x => x.ID_BaoCao);
                });

            migrationBuilder.CreateTable(
                name: "HoaDon_ThanhLys",
                columns: table => new
                {
                    ID_HoaDonThanhLy = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_ThietBi = table.Column<int>(type: "int", nullable: false),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NgayThanhLy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ID_User = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDon_ThanhLys", x => x.ID_HoaDonThanhLy);
                });

            migrationBuilder.CreateTable(
                name: "HoaDon_ThanhToans",
                columns: table => new
                {
                    ID_HoaDon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_TheHoiVien = table.Column<int>(type: "int", nullable: false),
                    ID_GoiTap = table.Column<int>(type: "int", nullable: false),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NgayThanhToan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ID_User = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDon_ThanhToans", x => x.ID_HoaDon);
                    table.ForeignKey(
                        name: "FK_HoaDon_ThanhToans_GoiTaps_ID_GoiTap",
                        column: x => x.ID_GoiTap,
                        principalTable: "GoiTaps",
                        principalColumn: "ID_GoiTap",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HoiViens",
                columns: table => new
                {
                    ID_HoiVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sdt = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ID_User = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoiViens", x => x.ID_HoiVien);
                });

            migrationBuilder.CreateTable(
                name: "LichTaps",
                columns: table => new
                {
                    ID_LichTap = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_TheHoiVien = table.Column<int>(type: "int", nullable: false),
                    ID_PhongTap = table.Column<int>(type: "int", nullable: false),
                    NgayTap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianTap = table.Column<TimeSpan>(type: "time", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ID_User = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichTaps", x => x.ID_LichTap);
                });

            migrationBuilder.CreateTable(
                name: "PhanCongs",
                columns: table => new
                {
                    ID_PhanCong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_PhongTap = table.Column<int>(type: "int", nullable: false),
                    ID_User = table.Column<int>(type: "int", nullable: false),
                    ID_Ca = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByAdminID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhanCongs", x => x.ID_PhanCong);
                    table.ForeignKey(
                        name: "FK_PhanCongs_CaLamViecs_ID_Ca",
                        column: x => x.ID_Ca,
                        principalTable: "CaLamViecs",
                        principalColumn: "ID_Ca",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID_User = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_PhanCong = table.Column<int>(type: "int", nullable: true),
                    HoTen = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Sdt = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ID_Role = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID_User);
                    table.ForeignKey(
                        name: "FK_Users_PhanCongs_ID_PhanCong",
                        column: x => x.ID_PhanCong,
                        principalTable: "PhanCongs",
                        principalColumn: "ID_PhanCong",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Roles_ID_Role",
                        column: x => x.ID_Role,
                        principalTable: "Roles",
                        principalColumn: "ID_Role",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhongTaps",
                columns: table => new
                {
                    ID_PhongTap = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenPhong = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SucChua = table.Column<int>(type: "int", nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ID_User = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhongTaps", x => x.ID_PhongTap);
                    table.ForeignKey(
                        name: "FK_PhongTaps_Users_ID_User",
                        column: x => x.ID_User,
                        principalTable: "Users",
                        principalColumn: "ID_User",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuanLy_NhanViens",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Admin = table.Column<int>(type: "int", nullable: false),
                    ID_User = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuanLy_NhanViens", x => x.ID);
                    table.ForeignKey(
                        name: "FK_QuanLy_NhanViens_Users_ID_Admin",
                        column: x => x.ID_Admin,
                        principalTable: "Users",
                        principalColumn: "ID_User",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuanLy_NhanViens_Users_ID_User",
                        column: x => x.ID_User,
                        principalTable: "Users",
                        principalColumn: "ID_User",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TheHoiViens",
                columns: table => new
                {
                    ID_TheHoiVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_HoiVien = table.Column<int>(type: "int", nullable: false),
                    ID_GoiTap = table.Column<int>(type: "int", nullable: false),
                    LoaiTheHoiVien = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayHetHan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ID_User = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheHoiViens", x => x.ID_TheHoiVien);
                    table.ForeignKey(
                        name: "FK_TheHoiViens_GoiTaps_ID_GoiTap",
                        column: x => x.ID_GoiTap,
                        principalTable: "GoiTaps",
                        principalColumn: "ID_GoiTap",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TheHoiViens_HoiViens_ID_HoiVien",
                        column: x => x.ID_HoiVien,
                        principalTable: "HoiViens",
                        principalColumn: "ID_HoiVien",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TheHoiViens_Users_ID_User",
                        column: x => x.ID_User,
                        principalTable: "Users",
                        principalColumn: "ID_User",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ThietBis",
                columns: table => new
                {
                    ID_ThietBi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Phong = table.Column<int>(type: "int", nullable: false),
                    TenThietBi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    XuatXu = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BaoHanh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TinhTrang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ID_User = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThietBis", x => x.ID_ThietBi);
                    table.ForeignKey(
                        name: "FK_ThietBis_PhongTaps_ID_Phong",
                        column: x => x.ID_Phong,
                        principalTable: "PhongTaps",
                        principalColumn: "ID_PhongTap",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThietBis_Users_ID_User",
                        column: x => x.ID_User,
                        principalTable: "Users",
                        principalColumn: "ID_User",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaos_ID_User",
                table: "BaoCaos",
                column: "ID_User");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_ThanhLys_ID_ThietBi",
                table: "HoaDon_ThanhLys",
                column: "ID_ThietBi");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_ThanhLys_ID_User",
                table: "HoaDon_ThanhLys",
                column: "ID_User");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_ThanhToans_ID_GoiTap",
                table: "HoaDon_ThanhToans",
                column: "ID_GoiTap");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_ThanhToans_ID_TheHoiVien",
                table: "HoaDon_ThanhToans",
                column: "ID_TheHoiVien");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_ThanhToans_ID_User",
                table: "HoaDon_ThanhToans",
                column: "ID_User");

            migrationBuilder.CreateIndex(
                name: "IX_HoiViens_ID_User",
                table: "HoiViens",
                column: "ID_User");

            migrationBuilder.CreateIndex(
                name: "IX_LichTaps_ID_PhongTap",
                table: "LichTaps",
                column: "ID_PhongTap");

            migrationBuilder.CreateIndex(
                name: "IX_LichTaps_ID_TheHoiVien",
                table: "LichTaps",
                column: "ID_TheHoiVien");

            migrationBuilder.CreateIndex(
                name: "IX_LichTaps_ID_User",
                table: "LichTaps",
                column: "ID_User");

            migrationBuilder.CreateIndex(
                name: "IX_PhanCongs_CreatedByAdminID",
                table: "PhanCongs",
                column: "CreatedByAdminID");

            migrationBuilder.CreateIndex(
                name: "IX_PhanCongs_ID_Ca",
                table: "PhanCongs",
                column: "ID_Ca");

            migrationBuilder.CreateIndex(
                name: "IX_PhanCongs_ID_PhongTap",
                table: "PhanCongs",
                column: "ID_PhongTap");

            migrationBuilder.CreateIndex(
                name: "IX_PhanCongs_ID_User",
                table: "PhanCongs",
                column: "ID_User");

            migrationBuilder.CreateIndex(
                name: "IX_PhongTaps_ID_User",
                table: "PhongTaps",
                column: "ID_User");

            migrationBuilder.CreateIndex(
                name: "IX_QuanLy_NhanViens_ID_Admin",
                table: "QuanLy_NhanViens",
                column: "ID_Admin");

            migrationBuilder.CreateIndex(
                name: "IX_QuanLy_NhanViens_ID_User",
                table: "QuanLy_NhanViens",
                column: "ID_User");

            migrationBuilder.CreateIndex(
                name: "IX_TheHoiViens_ID_GoiTap",
                table: "TheHoiViens",
                column: "ID_GoiTap");

            migrationBuilder.CreateIndex(
                name: "IX_TheHoiViens_ID_HoiVien",
                table: "TheHoiViens",
                column: "ID_HoiVien");

            migrationBuilder.CreateIndex(
                name: "IX_TheHoiViens_ID_User",
                table: "TheHoiViens",
                column: "ID_User");

            migrationBuilder.CreateIndex(
                name: "IX_ThietBis_ID_Phong",
                table: "ThietBis",
                column: "ID_Phong");

            migrationBuilder.CreateIndex(
                name: "IX_ThietBis_ID_User",
                table: "ThietBis",
                column: "ID_User");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ID_PhanCong",
                table: "Users",
                column: "ID_PhanCong");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ID_Role",
                table: "Users",
                column: "ID_Role");

            migrationBuilder.AddForeignKey(
                name: "FK_BaoCaos_Users_ID_User",
                table: "BaoCaos",
                column: "ID_User",
                principalTable: "Users",
                principalColumn: "ID_User",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_ThanhLys_ThietBis_ID_ThietBi",
                table: "HoaDon_ThanhLys",
                column: "ID_ThietBi",
                principalTable: "ThietBis",
                principalColumn: "ID_ThietBi",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_ThanhLys_Users_ID_User",
                table: "HoaDon_ThanhLys",
                column: "ID_User",
                principalTable: "Users",
                principalColumn: "ID_User",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_ThanhToans_TheHoiViens_ID_TheHoiVien",
                table: "HoaDon_ThanhToans",
                column: "ID_TheHoiVien",
                principalTable: "TheHoiViens",
                principalColumn: "ID_TheHoiVien",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_ThanhToans_Users_ID_User",
                table: "HoaDon_ThanhToans",
                column: "ID_User",
                principalTable: "Users",
                principalColumn: "ID_User",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoiViens_Users_ID_User",
                table: "HoiViens",
                column: "ID_User",
                principalTable: "Users",
                principalColumn: "ID_User",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LichTaps_PhongTaps_ID_PhongTap",
                table: "LichTaps",
                column: "ID_PhongTap",
                principalTable: "PhongTaps",
                principalColumn: "ID_PhongTap",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LichTaps_TheHoiViens_ID_TheHoiVien",
                table: "LichTaps",
                column: "ID_TheHoiVien",
                principalTable: "TheHoiViens",
                principalColumn: "ID_TheHoiVien",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LichTaps_Users_ID_User",
                table: "LichTaps",
                column: "ID_User",
                principalTable: "Users",
                principalColumn: "ID_User",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PhanCongs_PhongTaps_ID_PhongTap",
                table: "PhanCongs",
                column: "ID_PhongTap",
                principalTable: "PhongTaps",
                principalColumn: "ID_PhongTap",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PhanCongs_Users_CreatedByAdminID",
                table: "PhanCongs",
                column: "CreatedByAdminID",
                principalTable: "Users",
                principalColumn: "ID_User",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PhanCongs_Users_ID_User",
                table: "PhanCongs",
                column: "ID_User",
                principalTable: "Users",
                principalColumn: "ID_User",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhanCongs_Users_CreatedByAdminID",
                table: "PhanCongs");

            migrationBuilder.DropForeignKey(
                name: "FK_PhanCongs_Users_ID_User",
                table: "PhanCongs");

            migrationBuilder.DropForeignKey(
                name: "FK_PhongTaps_Users_ID_User",
                table: "PhongTaps");

            migrationBuilder.DropTable(
                name: "BaoCaos");

            migrationBuilder.DropTable(
                name: "HoaDon_ThanhLys");

            migrationBuilder.DropTable(
                name: "HoaDon_ThanhToans");

            migrationBuilder.DropTable(
                name: "LichTaps");

            migrationBuilder.DropTable(
                name: "QuanLy_NhanViens");

            migrationBuilder.DropTable(
                name: "ThietBis");

            migrationBuilder.DropTable(
                name: "TheHoiViens");

            migrationBuilder.DropTable(
                name: "GoiTaps");

            migrationBuilder.DropTable(
                name: "HoiViens");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "PhanCongs");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "CaLamViecs");

            migrationBuilder.DropTable(
                name: "PhongTaps");
        }
    }
}
