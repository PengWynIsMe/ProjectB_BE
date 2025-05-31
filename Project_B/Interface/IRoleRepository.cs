using Project_B.Models;

namespace Project_B.Interface
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int id);
        Task AddRoleAsync(Role role);
        Task UpdateRoleAsync(Role role);
        Task DeleteRoleAsync(int id);

        Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId);
        Task<bool> AssignUserToRoleAsync(int roleId, int userId);
        Task<bool> RemoveUserFromRoleAsync(int roleId, int userId);

        //Add
        Task<IEnumerable<User>> GetUnassignedUsersAsync(int roleId);

    }
}
