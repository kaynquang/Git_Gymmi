using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gymmi.Models
{
    public class LichTap
    {
        [Key]
        public int ID_LichTap { get; set; }
        
        public int ID_TheHoiVien { get; set; }
        
        public int ID_PhongTap { get; set; }
        
        public DateTime NgayTap { get; set; }
        
        public TimeSpan ThoiGianTap { get; set; }
        
        [StringLength(1000)]
        public string? MoTa { get; set; }
        
        public int ID_User { get; set; }
        
        // Navigation properties
        [ForeignKey("ID_TheHoiVien")]
        public virtual TheHoiVien TheHoiVien { get; set; }
        
        [ForeignKey("ID_PhongTap")]
        public virtual PhongTap PhongTap { get; set; }
        
        [ForeignKey("ID_User")]
        public virtual User User { get; set; }
    }
} 