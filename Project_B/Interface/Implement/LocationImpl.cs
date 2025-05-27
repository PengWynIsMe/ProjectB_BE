using Project_B.Data;
using Project_B.Models;
using Microsoft.EntityFrameworkCore;

namespace Project_B.Interface.Implement
{
    public class LocationImpl : ILocationRepository
    {
        private readonly AppDbContext _context;

        public LocationImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            return await _context.Locations.ToListAsync() ?? new List<Location>();
        }

        public async Task<Location?> GetLocationByIdAsync(int locationId)
        {
            return await _context.Locations.FirstOrDefaultAsync(l => l.LocationID == locationId);
        }

        public async Task AddLocationAsync(Location location)
        {
            await _context.Locations.AddAsync(location);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLocationAsync(Location location)
        {
            _context.Locations.Update(location);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLocationAsync(int locationId)
        {
            var location = await _context.Locations.FindAsync(locationId);
            if (location != null)
            {
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
            }
        }
    }
}
