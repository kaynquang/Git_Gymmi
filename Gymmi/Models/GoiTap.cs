using System.ComponentModel.DataAnnotations;

namespace Gymmi.Models
{
    public class GoiTap
    {
        [Key]
        public int ID_GoiTap { get; set; }
        
        [Required]
        [StringLength(200)]
        public string TenGoi { get; set; }
        
        public string TenGoiTap => TenGoi; // Alias for compatibility
        
        [Required]
        [StringLength(100)]
        public string LoaiGoiTap { get; set; }
        
        [Required]
        public decimal GiaTien { get; set; }
        
        public DateTime NgayBatDau { get; set; }
        
        public DateTime NgayKetThuc { get; set; }
        
        public int SoNgay { get; set; }
        
        public int ThoiHan => SoNgay; // Alias for compatibility
        
        [StringLength(1000)]
        public string? MoTa { get; set; }
        
        // Navigation properties
        public virtual ICollection<TheHoiVien> TheHoiViens { get; set; } = new List<TheHoiVien>();
        public virtual ICollection<HoaDon_ThanhToan> HoaDon_ThanhToans { get; set; } = new List<HoaDon_ThanhToan>();
    }
} 