using System.ComponentModel.DataAnnotations;

namespace Project_B.DTOs
{
    public class RoleAssignDto
    {
        [Required]
        public int UserId { get; set; }
    }
}
