using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gymmi.Models
{
    public class HoiVien
    {
        [Key]
        public int ID_HoiVien { get; set; }
        
        [Required]
        [StringLength(200)]
        public string HoTen { get; set; }
        
        public DateTime NgaySinh { get; set; }
        
        [Required]
        [StringLength(15)]
        public string Sdt { get; set; }
        
        public string SoDienThoai => Sdt; // Alias for compatibility
        
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }
        
        [StringLength(500)]
        public string? DiaChi { get; set; }
        
        [StringLength(10)]
        public string? GioiTinh { get; set; }
        
        public DateTime NgayThamGia { get; set; } = DateTime.Now;
        
        // Alias for compatibility
        public DateTime NgayDangKy => NgayThamGia;
        
        [Required]
        [StringLength(50)]
        public string TrangThai { get; set; } = "Hoạt động";
        
        public int ID_User { get; set; }
        
        // Navigation properties
        [ForeignKey("ID_User")]
        public virtual User User { get; set; }
        
        public virtual ICollection<TheHoiVien> TheHoiViens { get; set; } = new List<TheHoiVien>();
    }
} 