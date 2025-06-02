using Project_B.Data;
using Project_B.Models;
using Microsoft.EntityFrameworkCore;


namespace Project_B.Interface.Implement
{
    public class MealImpl : IMealRepository
    {
        private readonly AppDbContext _context;

        public MealImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Meal>> GetAllMealsAsync()
        {
            return await _context.Meals.ToListAsync() ?? new List<Meal>();
        }

        public async Task<Meal?> GetMealByIdAsync(int mealId)
        {
            return await _context.Meals.FirstOrDefaultAsync(m => m.MealID == mealId);
        }

        public async Task AddMealAsync(Meal meal)
        {
            await _context.Meals.AddAsync(meal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMealAsync(Meal meal)
        {
            _context.Meals.Update(meal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMealAsync(int mealId)
        {
            var meal = await _context.Meals.FindAsync(mealId);
            if (meal != null)
            {
                _context.Meals.Remove(meal);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Meal>> GetMealsByUserIdAsync(int userId)
        {
            return await _context.Meals.Where(g => g.UserId == userId).ToListAsync() ?? new List<Meal>();
        }
    }
}
