using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gymmi.Models
{
    public class HoaDon_ThanhLy
    {
        [Key]
        public int ID_HoaDonThanhLy { get; set; }
        
        public int ID_ThietBi { get; set; }
        
        [Required]
        public decimal SoTien { get; set; }
        
        public DateTime NgayThanhLy { get; set; }
        
        public int ID_User { get; set; }
        
        // Navigation properties
        [ForeignKey("ID_ThietBi")]
        public virtual ThietBi ThietBi { get; set; }
        
        [ForeignKey("ID_User")]
        public virtual User User { get; set; }
    }
} 