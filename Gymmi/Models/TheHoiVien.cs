using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gymmi.Models
{
    public class TheHoiVien
    {
        [Key]
        public int ID_TheHoiVien { get; set; }
        
        public int ID_HoiVien { get; set; }
        
        public int ID_GoiTap { get; set; }
        
        [Required]
        [StringLength(100)]
        public string LoaiTheHoiVien { get; set; }
        
        public DateTime NgayBatDau { get; set; }
        
        public DateTime NgayHetHan { get; set; }
        
        [Required]
        [StringLength(50)]
        public string TrangThai { get; set; }
        
        public int ID_User { get; set; }
        
        // Navigation properties
        [ForeignKey("ID_HoiVien")]
        public virtual HoiVien HoiVien { get; set; }
        
        [ForeignKey("ID_GoiTap")]
        public virtual GoiTap GoiTap { get; set; }
        
        [ForeignKey("ID_User")]
        public virtual User User { get; set; }
        
        public virtual ICollection<LichTap> LichTaps { get; set; } = new List<LichTap>();
        public virtual ICollection<HoaDon_ThanhToan> HoaDon_ThanhToans { get; set; } = new List<HoaDon_ThanhToan>();
    }
} 