using Project_B.Data;
using Project_B.Models;
using Microsoft.EntityFrameworkCore;


namespace Project_B.Interface.Implement
{
    public class EventImpl : IEventRepository
    {
        private readonly AppDbContext _context;
        public EventImpl(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _context.Events.ToListAsync() ?? Enumerable.Empty<Event>();
        }
        public async Task<Event?> GetEventByIdAsync(int id)
        {
            return await _context.Events.FindAsync(id);
        }
        public async Task AddEventAsync(Event eventEntity)
        {
            await _context.Events.AddAsync(eventEntity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateEventAsync(Event eventEntity)
        {
            _context.Events.Update(eventEntity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteEventAsync(int id)
        {
            var eventEntity = await _context.Events.FindAsync(id);
            if (eventEntity != null)
            {
                _context.Events.Remove(eventEntity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Event>> GetEventsByUserIdAsync(int userId)
        {
            return await _context.Events
                .Where(e => e.UserId == userId)
                .ToListAsync() ?? Enumerable.Empty<Event>();
        }
    }
}

