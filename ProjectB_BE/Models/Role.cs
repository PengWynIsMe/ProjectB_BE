using System.ComponentModel.DataAnnotations;

namespace Project_B.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        public int Status { get; set; }

        public DateTime CreatedDate { get; set; }

        [MaxLength(50)]
        public required string RoleName { get; set; }

        public ICollection<RoleUser>? RoleUsers { get; set; }
    }
}
