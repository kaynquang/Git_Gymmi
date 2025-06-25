using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gymmi.Models
{
    public class PhongTap
    {
        [Key]
        public int ID_PhongTap { get; set; }
        
        [Required]
        [StringLength(200)]
        public string TenPhong { get; set; }
        
        [Required]
        [StringLength(500)]
        public string DiaChi { get; set; }
        
        [StringLength(200)]
        public string? ViTri { get; set; }
        
        public int SucChua { get; set; }
        
        // Slot management
        public int SlotToiDa { get; set; } = 20; // Số slot tối đa mỗi phòng
        public int SlotDaDangKy { get; set; } = 0; // Số slot đã được đăng ký
        
        [NotMapped]
        public int SlotConLai => SlotToiDa - SlotDaDangKy;
        
        [NotMapped]
        public bool CoSlotTrong => SlotConLai > 0;
        
        [Required]
        [StringLength(100)]
        public string TinhTrang { get; set; }
        
        public int ID_User { get; set; }
        
        // Trainer assignment for sessions
        public int? ID_TrainerPhuTrach { get; set; }
        
        // Navigation properties
        [ForeignKey("ID_User")]
        public virtual User User { get; set; }
        
        [ForeignKey("ID_TrainerPhuTrach")]
        public virtual User? TrainerPhuTrach { get; set; }
        
        public virtual ICollection<ThietBi> ThietBis { get; set; } = new List<ThietBi>();
        public virtual ICollection<LichTap> LichTaps { get; set; } = new List<LichTap>();
        public virtual ICollection<PhanCong> PhanCongs { get; set; } = new List<PhanCong>();
    }
} 