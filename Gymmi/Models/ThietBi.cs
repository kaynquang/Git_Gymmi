using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gymmi.Models
{
    public class ThietBi
    {
        [Key]
        public int ID_ThietBi { get; set; }
        
        public int ID_Phong { get; set; }
        
        [Required]
        [StringLength(200)]
        public string TenThietBi { get; set; }
        
        public int SoLuong { get; set; }
        
        [StringLength(200)]
        public string? XuatXu { get; set; }
        
        public DateTime? BaoHanh { get; set; }
        
        [Required]
        [StringLength(100)]
        public string TinhTrang { get; set; }
        
        public string TrangThai => TinhTrang; // Alias for compatibility
        
        [StringLength(1000)]
        public string? MoTa { get; set; }
        
        public DateTime NgayMua { get; set; } = DateTime.Now;
        
        public int ID_User { get; set; }
        
        // Navigation properties
        [ForeignKey("ID_Phong")]
        public virtual PhongTap PhongTap { get; set; }
        
        [ForeignKey("ID_User")]
        public virtual User User { get; set; }
        
        public virtual ICollection<HoaDon_ThanhLy> HoaDon_ThanhLys { get; set; } = new List<HoaDon_ThanhLy>();
    }
} 