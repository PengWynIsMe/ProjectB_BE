using Project_B.Models;

namespace Project_B.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Password { get; set; }
        public string Token { get; internal set; }
        public string Description { get; set; }
        public ICollection<RoleDTO> Roles { get; set; }

        // Constructor 
        public UserDTO(User user)
        {
            UserId = user.UserId;
            Name = user.Name;
            Email = user.Email;
            IsActive = user.IsActive;
            IsDeleted = user.IsDeleted;
            Password = user.Password;
            Description = user.Description;
            Roles = user.RoleUsers?
                .Where(ru => ru.Role != null)
                .Select(ru => new RoleDTO
                {
                    RoleId = ru.Role.RoleId,
                    RoleName = ru.Role.RoleName,
                    Status = ru.Role.Status
                }).ToList() ?? new List<RoleDTO>();
        }
    }
}

