using Project_B.Models;

namespace Project_B.Interface
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event?> GetEventByIdAsync(int id);
        Task AddEventAsync(Event eventEntity);
        Task UpdateEventAsync(Event eventEntity);
        Task DeleteEventAsync(int id);
        Task<IEnumerable<Event>> GetEventsByUserIdAsync(int userId);



    }
}
