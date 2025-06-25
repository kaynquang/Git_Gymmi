using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gymmi.Models
{
    public class QuanLy_NhanVien
    {
        [Key]
        public int ID { get; set; }
        
        public int ID_Admin { get; set; }
        
        public int ID_User { get; set; }
        
        // Navigation properties
        [ForeignKey("ID_Admin")]
        public virtual User Admin { get; set; }
        
        [ForeignKey("ID_User")]
        public virtual User User { get; set; }
    }
} 