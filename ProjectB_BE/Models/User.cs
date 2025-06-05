using System.ComponentModel.DataAnnotations;

namespace Project_B.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public required string Email { get; set; }

        public string? Password { get; set; }

        public string? Name { get; set; }

        public byte? Gender { get; set; }  // tinyint → byte

        public DateTime? DOB { get; set; } 

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public bool IsDeleted { get; set; }

        public int Status { get; set; }

        public DateTime CreatedTime { get; set; }

        public bool IsActive { get; set; }

        public string? OTPCode { get; set; }

        public string? Avatar { get; set; }

        public string? Description { get; set; }

        // Navigation properties
        public ICollection<RoleUser>? RoleUsers { get; set; }
        public ICollection<Budget>? Budgets { get; set; }
        public ICollection<Goal>? Goals { get; set; }
        public ICollection<Meal>? Meals { get; set; }
        public ICollection<Location>? Locations { get; set; }
    }
}
