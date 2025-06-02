using Microsoft.AspNetCore.Mvc;
using Project_B.Interface;
using Project_B.Models;

namespace Project_B.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoalsController : ControllerBase
    {
        private readonly IGoalRepository _goalRepository;

        public GoalsController(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Goal>>> GetAllGoals()
        {
            var goals = await _goalRepository.GetAllGoalsAsync();
            return Ok(goals);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Goal>> GetGoalById(int id)
        {
            var goal = await _goalRepository.GetGoalByIdAsync(id);
            if (goal == null)
                return NotFound();
            return Ok(goal);
        }

        [HttpPost]
        public async Task<ActionResult> AddGoal(Goal goal)
        {
            await _goalRepository.AddGoalAsync(goal);
            return CreatedAtAction(nameof(GetGoalById), new { id = goal.GoalID }, goal);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateGoal(int id, Goal goal)
        {
            if (id != goal.GoalID)
                return BadRequest();

            await _goalRepository.UpdateGoalAsync(goal);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGoal(int id)
        {
            await _goalRepository.DeleteGoalAsync(id);
            return NoContent();
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Goal>>> GetGoalsByUserId(int userId)
        {
            var goals = await _goalRepository.GetGoalsByUserIdAsync(userId);
            return Ok(goals);
        }

    }
}
