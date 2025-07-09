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
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> AddLocation([FromForm] LocationDTO request)
        {
            string? imagePath = null;
            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "UserLocationPhotos");
                Directory.CreateDirectory(folder);
                var fileName = Guid.NewGuid() + Path.GetExtension(request.ImageFile.FileName);
                var fullPath = Path.Combine(folder, fileName);
                using var fs = new FileStream(fullPath, FileMode.Create);
                await request.ImageFile.CopyToAsync(fs);
                imagePath = Path.Combine("Assets", "UserLocationPhotos", fileName).Replace("\\", "/");
            }

            var loc = new Location
            {
                LocationName = request.LocationName,
                Address = request.Address,
                OpenTime = request.OpenTime,
                CloseTime = request.CloseTime,
                Phone = request.Phone,
                Note = request.Note,
                UserId = request.UserId,
                Image = imagePath
            };

            await _locationRepository.AddLocationAsync(loc);
            return CreatedAtAction(nameof(GetLocationById), new { id = loc.LocationID }, loc);
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
