using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_B.Models
{
    public class RoleUser
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public Role? Role { get; set; } 
        public User? User { get; set; } 
    }
}
