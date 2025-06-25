using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gymmi.Models;
using Microsoft.EntityFrameworkCore;

namespace Gymmi.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure User relationships
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.ID_Role)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.PhanCong)
                .WithMany(p => p.AssignedUsers)
                .HasForeignKey(u => u.ID_PhanCong)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure PhanCong relationships
            modelBuilder.Entity<PhanCong>()
                .HasOne(p => p.User)
                .WithMany(u => u.PhanCongs)
                .HasForeignKey(p => p.ID_User)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhanCong>()
                .HasOne(p => p.CreatedByAdmin)
                .WithMany(u => u.CreatedPhanCongs)
                .HasForeignKey(p => p.CreatedByAdminID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhanCong>()
                .HasOne(p => p.PhongTap)
                .WithMany(pt => pt.PhanCongs)
                .HasForeignKey(p => p.ID_PhongTap)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure QuanLy_NhanVien relationships to prevent cascade conflicts
            modelBuilder.Entity<QuanLy_NhanVien>()
                .HasOne(q => q.Admin)
                .WithMany()
                .HasForeignKey(q => q.ID_Admin)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuanLy_NhanVien>()
                .HasOne(q => q.User)
                .WithMany()
                .HasForeignKey(q => q.ID_User)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure HoiVien relationships
            modelBuilder.Entity<HoiVien>()
                .HasOne(h => h.User)
                .WithMany(u => u.HoiViens)
                .HasForeignKey(h => h.ID_User)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure TheHoiVien relationships
            modelBuilder.Entity<TheHoiVien>()
                .HasOne(t => t.User)
                .WithMany(u => u.TheHoiViens)
                .HasForeignKey(t => t.ID_User)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TheHoiVien>()
                .HasOne(t => t.HoiVien)
                .WithMany(h => h.TheHoiViens)
                .HasForeignKey(t => t.ID_HoiVien)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TheHoiVien>()
                .HasOne(t => t.GoiTap)
                .WithMany(g => g.TheHoiViens)
                .HasForeignKey(t => t.ID_GoiTap)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LichTap>()
                .HasOne(l => l.User)
                .WithMany(u => u.LichTaps)
                .HasForeignKey(l => l.ID_User)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LichTap>()
                .HasOne(l => l.PhongTap)
                .WithMany(p => p.LichTaps)
                .HasForeignKey(l => l.ID_PhongTap)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhongTap>()
                .HasOne(p => p.User)
                .WithMany(u => u.PhongTaps)
                .HasForeignKey(p => p.ID_User)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure ThietBi relationships
            modelBuilder.Entity<ThietBi>()
                .HasOne(t => t.User)
                .WithMany(u => u.ThietBis)
                .HasForeignKey(t => t.ID_User)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ThietBi>()
                .HasOne(t => t.PhongTap)
                .WithMany(p => p.ThietBis)
                .HasForeignKey(t => t.ID_Phong)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure HoaDon_ThanhToan relationships
            modelBuilder.Entity<HoaDon_ThanhToan>()
                .HasOne(h => h.User)
                .WithMany(u => u.HoaDon_ThanhToans)
                .HasForeignKey(h => h.ID_User)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HoaDon_ThanhToan>()
                .HasOne(h => h.TheHoiVien)
                .WithMany(t => t.HoaDon_ThanhToans)
                .HasForeignKey(h => h.ID_TheHoiVien)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HoaDon_ThanhToan>()
                .HasOne(h => h.GoiTap)
                .WithMany(g => g.HoaDon_ThanhToans)
                .HasForeignKey(h => h.ID_GoiTap)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure HoaDon_ThanhLy relationships
            modelBuilder.Entity<HoaDon_ThanhLy>()
                .HasOne(h => h.User)
                .WithMany(u => u.HoaDon_ThanhLys)
                .HasForeignKey(h => h.ID_User)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HoaDon_ThanhLy>()
                .HasOne(h => h.ThietBi)
                .WithMany(t => t.HoaDon_ThanhLys)
                .HasForeignKey(h => h.ID_ThietBi)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BaoCao>()
                .HasOne(b => b.User)
                .WithMany(u => u.BaoCaos)
                .HasForeignKey(b => b.ID_User)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure decimal precision
            modelBuilder.Entity<GoiTap>()
                .Property(g => g.GiaTien)
                .HasPrecision(18, 2);

            modelBuilder.Entity<HoaDon_ThanhToan>()
                .Property(h => h.SoTien)
                .HasPrecision(18, 2);

            modelBuilder.Entity<HoaDon_ThanhLy>()
                .Property(h => h.SoTien)
                .HasPrecision(18, 2);

            // Seed data
            modelBuilder.Entity<Role>().HasData(
                new Role { ID_Role = 1, TenRole = "Admin", MoTa = "Quản trị viên hệ thống" },
                new Role { ID_Role = 2, TenRole = "Nhân viên", MoTa = "Nhân viên phòng tập" },
                new Role { ID_Role = 3, TenRole = "Hội viên", MoTa = "Thành viên phòng tập" },
                new Role { ID_Role = 4, TenRole = "Huấn luyện viên", MoTa = "Huấn luyện viên cá nhân" }
            );

            modelBuilder.Entity<CaLamViec>().HasData(
                new CaLamViec { ID_Ca = 1, TenCa = "Ca sáng", MoTa = "6:00 - 14:00" },
                new CaLamViec { ID_Ca = 2, TenCa = "Ca chiều", MoTa = "14:00 - 22:00" },
                new CaLamViec { ID_Ca = 3, TenCa = "Ca tối", MoTa = "18:00 - 02:00" }
            );

            base.OnModelCreating(modelBuilder);
        }
        
        // DbSets for all models
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<HoiVien> HoiViens { get; set; }
        public DbSet<TheHoiVien> TheHoiViens { get; set; }
        public DbSet<GoiTap> GoiTaps { get; set; }
        public DbSet<LichTap> LichTaps { get; set; }
        public DbSet<PhongTap> PhongTaps { get; set; }
        public DbSet<ThietBi> ThietBis { get; set; }
        public DbSet<HoaDon_ThanhToan> HoaDon_ThanhToans { get; set; }
        public DbSet<HoaDon_ThanhLy> HoaDon_ThanhLys { get; set; }
        public DbSet<BaoCao> BaoCaos { get; set; }
        public DbSet<PhanCong> PhanCongs { get; set; }
        public DbSet<CaLamViec> CaLamViecs { get; set; }
        public DbSet<QuanLy_NhanVien> QuanLy_NhanViens { get; set; }
    }
}