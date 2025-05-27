using System.ComponentModel.DataAnnotations;

namespace Project_B.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        public int Status { get; set; }

        public DateTime CreatedDate { get; set; }

        [MaxLength(50)]
        public string RoleName { get; set; }

        public List<RoleUser> RoleUsers { get; set; }
    }
}
