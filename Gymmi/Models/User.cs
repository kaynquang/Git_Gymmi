using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gymmi.Models
{
    public class User
    {
        [Key]
        public int ID_User { get; set; }
        
        public int? ID_PhanCong { get; set; }
        
        [Required]
        [StringLength(200)]
        public string HoTen { get; set; }
        
        [Required]
        [StringLength(50)]
        public string TenDangNhap { get; set; }
        
        [Required]
        [StringLength(15)]
        public string Sdt { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Password { get; set; }
        
        [Required]
        public int ID_Role { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        [ForeignKey("ID_Role")]
        public virtual Role Role { get; set; }
        
        [ForeignKey("ID_PhanCong")]
        public virtual PhanCong? PhanCong { get; set; }
        
        // Reverse navigation properties
        public virtual ICollection<HoiVien> HoiViens { get; set; } = new List<HoiVien>();
        public virtual ICollection<TheHoiVien> TheHoiViens { get; set; } = new List<TheHoiVien>();
        public virtual ICollection<LichTap> LichTaps { get; set; } = new List<LichTap>();
        public virtual ICollection<PhongTap> PhongTaps { get; set; } = new List<PhongTap>();
        public virtual ICollection<ThietBi> ThietBis { get; set; } = new List<ThietBi>();
        public virtual ICollection<HoaDon_ThanhToan> HoaDon_ThanhToans { get; set; } = new List<HoaDon_ThanhToan>();
        public virtual ICollection<HoaDon_ThanhLy> HoaDon_ThanhLys { get; set; } = new List<HoaDon_ThanhLy>();
        public virtual ICollection<BaoCao> BaoCaos { get; set; } = new List<BaoCao>();
        public virtual ICollection<PhanCong> PhanCongs { get; set; } = new List<PhanCong>();
        public virtual ICollection<PhanCong> CreatedPhanCongs { get; set; } = new List<PhanCong>();
    }
}