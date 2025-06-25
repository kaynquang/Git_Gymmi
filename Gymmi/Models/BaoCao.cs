using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gymmi.Models
{
    public class BaoCao
    {
        [Key]
        public int ID_BaoCao { get; set; }
        
        [Required]
        [StringLength(100)]
        public string LoaiBaoCao { get; set; }
        
        [StringLength(200)]
        public string? TenBaoCao { get; set; }
        
        public DateTime NgayBatDau { get; set; }
        
        public DateTime NgayTao { get; set; } = DateTime.Now;
        
        [Required]
        [StringLength(2000)]
        public string NoiDung { get; set; }
        
        public int ID_User { get; set; }
        
        // Navigation properties
        [ForeignKey("ID_User")]
        public virtual User User { get; set; }
    }
} 