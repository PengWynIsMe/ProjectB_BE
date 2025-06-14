using Project_B.Models;

namespace Project_B.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte? Gender { get; set; }

        public string? Avatar { get; set; }

        public DateTime? DOB { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public DateTime CreatedTime { get; set; }
        public bool IsActive { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }
        public string Password { get; set; }
        public string Token { get; internal set; }
        public bool HasPassword { get; set; }
        public string Description { get; set; }
        public ICollection<String> Roles { get; set; }

        public UserDTO() { }

        // Constructor 
        public UserDTO(User user)
        {
            UserId = user.UserId;
            Name = user.Name;
            Email = user.Email;
            HasPassword = !string.IsNullOrEmpty(user.Password);
            IsActive = user.IsActive;
            DOB = user.DOB;
            Phone = user.Phone;
            Address = user.Address;
            Gender = user.Gender;
            Status = user.Status;
            IsDeleted = user.IsDeleted;
            CreatedTime = user.CreatedTime;
            Password = user.Password;
            Description = user.Description;
            Roles = user.RoleUsers?
                .Where(ru => ru.Role != null)
                .Select(ru => ru.Role.RoleName)
                .ToList() ?? new List<String>();
        }
    }
}

