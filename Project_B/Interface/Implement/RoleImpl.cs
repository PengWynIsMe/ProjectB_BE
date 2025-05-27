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
                                       .FirstOrDefaultAsync(r => r.Id == id);
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
    }
}
