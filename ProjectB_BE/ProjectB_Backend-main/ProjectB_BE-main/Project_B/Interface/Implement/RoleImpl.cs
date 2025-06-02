using Project_B.Data;
using Project_B.Models;
using Microsoft.EntityFrameworkCore;


namespace Project_B.Interface.Implement
{
    public class RoleImpl : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleImpl(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.Include(r => r.RoleUsers).ToListAsync() ?? Enumerable.Empty<Role>();
        }

        public async Task<Role?> GetRoleByIdAsync(int id)
        {
            return await _context.Roles.Include(r => r.RoleUsers)
                                       .FirstOrDefaultAsync(r => r.RoleId == id);
        }
        public async Task AddRoleAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoleAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }

        // Management 
        public async Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId)
        {
            return await _context.RoleUsers
                .Where(ru => ru.RoleId == roleId)
                .Select(ru => ru.User!)
                .ToListAsync();
        }

        public async Task<bool> AssignUserToRoleAsync(int roleId, int userId)
        {
            var exists = await _context.RoleUsers
                .AnyAsync(ru => ru.RoleId == roleId && ru.UserId == userId);

            if (exists) return false;

            _context.RoleUsers.Add(new RoleUser
            {
                RoleId = roleId,
                UserId = userId
            });

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveUserFromRoleAsync(int roleId, int userId)
        {
            var roleUser = await _context.RoleUsers
                .FirstOrDefaultAsync(ru => ru.RoleId == roleId && ru.UserId == userId);

            if (roleUser == null) return false;

            _context.RoleUsers.Remove(roleUser);
            return await _context.SaveChangesAsync() > 0;
        }

        //Add
        public async Task<IEnumerable<User>> GetUnassignedUsersAsync(int roleId)
        {
            // Lấy danh sách userId đã có trong role
            var assignedUserIds = await _context.RoleUsers
                .Where(ru => ru.RoleId == roleId)
                .Select(ru => ru.UserId)
                .ToListAsync();

            // Trả về user chưa có trong danh sách đó
            return await _context.Users
                .Where(u => !assignedUserIds.Contains(u.UserId))
                .ToListAsync();
        }


    }
}
