using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gymmi.Models
{
    public class PhanCong
    {
        [Key]
        public int ID_PhanCong { get; set; }
        
        public int ID_PhongTap { get; set; }
        
        public int ID_User { get; set; }
        
        public int ID_Ca { get; set; }
        
        [Required]
        [StringLength(100)]
        public string TrangThai { get; set; }
        
        [StringLength(1000)]
        public string? MoTa { get; set; }
        
        public DateTime NgayPhanCong { get; set; } = DateTime.Now;
        
        public int CreatedByAdminID { get; set; }
        
        // Navigation properties
        [ForeignKey("ID_PhongTap")]
        public virtual PhongTap PhongTap { get; set; }
        
        [ForeignKey("ID_User")]
        public virtual User User { get; set; }
        
        [ForeignKey("ID_Ca")]
        public virtual CaLamViec CaLamViec { get; set; }
        
        [ForeignKey("CreatedByAdminID")]
        public virtual User CreatedByAdmin { get; set; }
        
        // Reverse navigation
        public virtual ICollection<User> AssignedUsers { get; set; } = new List<User>();
    }
} 