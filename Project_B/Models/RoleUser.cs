using System.ComponentModel.DataAnnotations.Schema;

namespace Project_B.Models
{
    public class RoleUser
    {
        public int Id { get; set; }

        // Đảm bảo RoleId được ánh xạ chính xác
        [ForeignKey("Role")]
        public int RoleId { get; set; }

        // Đảm bảo UserId được ánh xạ chính xác
        [ForeignKey("User")]
        public int UserId { get; set; }

        public Role Role { get; set; }  // Đối tượng Role
        public User User { get; set; }  // Đối tượng User
    }
}
