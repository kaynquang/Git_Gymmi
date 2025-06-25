using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymmi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelsForAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenDangNhap",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MoTa",
                table: "ThietBis",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayMua",
                table: "ThietBis",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ViTri",
                table: "PhongTaps",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayPhanCong",
                table: "PhanCongs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DiaChi",
                table: "HoiViens",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GioiTinh",
                table: "HoiViens",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayThamGia",
                table: "HoiViens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MoTa",
                table: "GoiTaps",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayTao",
                table: "BaoCaos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TenBaoCao",
                table: "BaoCaos",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenDangNhap",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MoTa",
                table: "ThietBis");

            migrationBuilder.DropColumn(
                name: "NgayMua",
                table: "ThietBis");

            migrationBuilder.DropColumn(
                name: "ViTri",
                table: "PhongTaps");

            migrationBuilder.DropColumn(
                name: "NgayPhanCong",
                table: "PhanCongs");

            migrationBuilder.DropColumn(
                name: "DiaChi",
                table: "HoiViens");

            migrationBuilder.DropColumn(
                name: "GioiTinh",
                table: "HoiViens");

            migrationBuilder.DropColumn(
                name: "NgayThamGia",
                table: "HoiViens");

            migrationBuilder.DropColumn(
                name: "MoTa",
                table: "GoiTaps");

            migrationBuilder.DropColumn(
                name: "NgayTao",
                table: "BaoCaos");

            migrationBuilder.DropColumn(
                name: "TenBaoCao",
                table: "BaoCaos");
        }
    }
}
