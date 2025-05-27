using Microsoft.AspNetCore.Mvc;
using Project_B.Interface;
using Project_B.Models;

namespace Project_B.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _locationRepository;

        public LocationController(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetAllLocations()
        {
            var locations = await _locationRepository.GetAllLocationsAsync();
            return Ok(locations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocationById(int id)
        {
            var location = await _locationRepository.GetLocationByIdAsync(id);
            if (location == null)
                return NotFound();
            return Ok(location);
        }

        [HttpPost]
        public async Task<ActionResult> AddLocation(Location location)
        {
            await _locationRepository.AddLocationAsync(location);
            return CreatedAtAction(nameof(GetLocationById), new { id = location.LocationID }, location);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateLocation(int id, Location location)
        {
            if (id != location.LocationID)
                return BadRequest();

            await _locationRepository.UpdateLocationAsync(location);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLocation(int id)
        {
            await _locationRepository.DeleteLocationAsync(id);
            return NoContent();
        }
    }
}
