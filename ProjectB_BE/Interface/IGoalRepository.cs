using Project_B.Models;

namespace Project_B.Interface
{
    public interface IGoalRepository
    {
        Task<IEnumerable<Goal>> GetAllGoalsAsync();
        Task<Goal?> GetGoalByIdAsync(int goalId);
        Task AddGoalAsync(Goal goal);
        Task UpdateGoalAsync(Goal goal);
        Task DeleteGoalAsync(int goalId);

        Task<IEnumerable<Goal>> GetGoalsByUserIdAsync(int userId);

    }
}
