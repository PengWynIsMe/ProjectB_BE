using Microsoft.AspNetCore.Mvc;
using Project_B.Interface;
using Project_B.Models;

namespace Project_B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventRepository.GetAllEventsAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var eventEntity = await _eventRepository.GetEventByIdAsync(id);
            if (eventEntity == null)
            {
                return NotFound();
            }
            return Ok(eventEntity);
        }

        [HttpPost]
        public async Task<IActionResult> AddEvent([FromBody] Event eventEntity)
        {
            await _eventRepository.AddEventAsync(eventEntity);
            return CreatedAtAction(nameof(GetEventById), new { id = eventEntity.EventID }, eventEntity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] Event eventEntity)
        {
            if (id != eventEntity.EventID)
            {
                return BadRequest();
            }
            await _eventRepository.UpdateEventAsync(eventEntity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            await _eventRepository.DeleteEventAsync(id);
            return NoContent();
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetEventsByUserId(int userId)
        {
            var events = await _eventRepository.GetEventsByUserIdAsync(userId);
            return Ok(events);
        }
    }
}
