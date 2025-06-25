using System.ComponentModel.DataAnnotations;

namespace Gymmi.Models
{
    public class Role
    {
        [Key]
        public int ID_Role { get; set; }
        
        [Required]
        [StringLength(100)]
        public string TenRole { get; set; }
        
        [StringLength(500)]
        public string? MoTa { get; set; }
        
        // Navigation properties
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
} 