using System.ComponentModel.DataAnnotations;

namespace Gymmi.Models
{
    public class CaLamViec
    {
        [Key]
        public int ID_Ca { get; set; }
        
        [Required]
        [StringLength(100)]
        public string TenCa { get; set; }
        
        [StringLength(500)]
        public string? MoTa { get; set; }
        
        // Navigation properties
        public virtual ICollection<PhanCong> PhanCongs { get; set; } = new List<PhanCong>();
    }
} 