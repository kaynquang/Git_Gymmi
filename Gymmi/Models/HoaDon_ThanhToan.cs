using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gymmi.Models
{
    public class HoaDon_ThanhToan
    {
        [Key]
        public int ID_HoaDon { get; set; }
        
        public int ID_TheHoiVien { get; set; }
        
        public int ID_GoiTap { get; set; }
        
        [Required]
        public decimal SoTien { get; set; }
        
        public DateTime NgayThanhToan { get; set; }
        
        public int ID_User { get; set; }
        
        // Navigation properties
        [ForeignKey("ID_TheHoiVien")]
        public virtual TheHoiVien TheHoiVien { get; set; }
        
        [ForeignKey("ID_GoiTap")]
        public virtual GoiTap GoiTap { get; set; }
        
        [ForeignKey("ID_User")]
        public virtual User User { get; set; }
    }
} 