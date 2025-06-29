﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymmi.Migrations
{
    /// <inheritdoc />
    public partial class AddTrangThaiToHoiVien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrangThai",
                table: "HoiViens",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "HoiViens");
        }
    }
}
