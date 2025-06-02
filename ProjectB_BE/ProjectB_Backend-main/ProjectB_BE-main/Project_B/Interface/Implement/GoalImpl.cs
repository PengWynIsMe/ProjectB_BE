using Project_B.Data;
using Project_B.Models;
using Microsoft.EntityFrameworkCore;


namespace Project_B.Interface.Implement
{
    public class GoalImpl : IGoalRepository
    {
        private readonly AppDbContext _context;

        public GoalImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Goal>> GetAllGoalsAsync()
        {
            return await _context.Goals.ToListAsync() ?? new List<Goal>();
        }

        public async Task<Goal?> GetGoalByIdAsync(int goalId)
        {
            return await _context.Goals.FirstOrDefaultAsync(g => g.GoalID == goalId);
        }

        public async Task AddGoalAsync(Goal goal)
        {
            await _context.Goals.AddAsync(goal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGoalAsync(Goal goal)
        {
            _context.Goals.Update(goal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGoalAsync(int goalId)
        {
            var goal = await _context.Goals.FindAsync(goalId);
            if (goal != null)
            {
                _context.Goals.Remove(goal);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Goal>> GetGoalsByUserIdAsync(int userId)
        {
            return await _context.Goals.Where(g => g.UserId == userId).ToListAsync() ?? new List<Goal>();
        }

    }
}

