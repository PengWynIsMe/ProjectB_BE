using Project_B.Models;

namespace Project_B.Interface
{
    public interface IMealRepository
    {
        Task<IEnumerable<Meal>> GetAllMealsAsync();
        Task<Meal?> GetMealByIdAsync(int mealId);
        Task AddMealAsync(Meal meal);
        Task UpdateMealAsync(Meal meal);
        Task DeleteMealAsync(int mealId);
        Task<IEnumerable<Meal>> GetMealsByUserIdAsync(int userId);
    }
}
