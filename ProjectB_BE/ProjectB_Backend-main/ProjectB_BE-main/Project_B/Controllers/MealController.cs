using Microsoft.AspNetCore.Mvc;
using Project_B.DTOs;
using Project_B.Interface;
using Project_B.Models;

namespace Project_B.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MealsController : ControllerBase
    {
        private readonly IMealRepository _mealRepository;

        public MealsController(IMealRepository mealRepository)
        {
            _mealRepository = mealRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MealDTO>>> GetAllMeals()
        {
            var meals = await _mealRepository.GetAllMealsAsync();
            var mealDTOs = new List<MealDTO>();

            foreach (var meal in meals)
                mealDTOs.Add(MapToDTO(meal));

            return Ok(mealDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MealDTO>> GetMealById(int id)
        {
            var meal = await _mealRepository.GetMealByIdAsync(id);
            if (meal == null) return NotFound();

            return Ok(MapToDTO(meal));
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> AddMeal([FromForm] MealDTO request)
        {
            string? imagePath = null;

            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "UserMealPhotos");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageFile.FileName);
                var filePath = Path.Combine(folderPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }

                imagePath = Path.Combine("Assets", "UserMealPhotos", uniqueFileName).Replace("\\", "/");
            }

            var meal = new Meal
            {
                MealName = request.MealName,
                Address = request.Address,
                OpenTime = request.OpenTime,
                Phone = request.Phone,
                Note = request.Note,
                UserId = request.UserId,
                Image = imagePath
            };

            await _mealRepository.AddMealAsync(meal);

            return CreatedAtAction(nameof(GetMealById), new { id = meal.MealID }, MapToDTO(meal));
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UpdateMeal(int id, [FromForm] MealDTO request)
        {
            if (request.MealID == null || id != request.MealID)
                return BadRequest();

            var meal = await _mealRepository.GetMealByIdAsync(id);
            if (meal == null) return NotFound();

            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "UserMealPhotos");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageFile.FileName);
                var filePath = Path.Combine(folderPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }

                meal.Image = Path.Combine("Assets", "UserMealPhotos", uniqueFileName).Replace("\\", "/");
            }

            meal.MealName = request.MealName;
            meal.Address = request.Address;
            meal.OpenTime = request.OpenTime;
            meal.Phone = request.Phone;
            meal.Note = request.Note;
            meal.UserId = request.UserId;

            await _mealRepository.UpdateMealAsync(meal);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMeal(int id)
        {
            var meal = await _mealRepository.GetMealByIdAsync(id);
            if (meal == null) return NotFound();

            await _mealRepository.DeleteMealAsync(id);
            return NoContent();
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<MealDTO>>> GetMealsByUserId(int userId)
        {
            var meals = await _mealRepository.GetMealsByUserIdAsync(userId);
            var mealDTOs = new List<MealDTO>();

            foreach (var meal in meals)
                mealDTOs.Add(MapToDTO(meal));

            return Ok(mealDTOs);
        }

        private MealDTO MapToDTO(Meal meal)
        {
            return new MealDTO
            {
                MealID = meal.MealID,
                MealName = meal.MealName,
                Address = meal.Address,
                OpenTime = meal.OpenTime,
                Phone = meal.Phone,
                Note = meal.Note,
                UserId = meal.UserId,
                Image = meal.Image,
                ImageFile = null
            };
        }
    }
}
