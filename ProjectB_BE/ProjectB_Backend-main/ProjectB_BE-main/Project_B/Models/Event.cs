using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_B.Models
{
    public class Event
    {
        public int EventID { get; set; }

        public int UserId { get; set; }

        [MaxLength(100)]
        public string EventName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? IconKey { get; set; }

        [MaxLength(50)]
        public string? Category { get; set; }

        public DateTime EventDate { get; set; }

        public string? Description { get; set; }

        public DateTime? NotificationTime { get; set; }

        public int Status { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public bool IsAllDay { get; set; }

        public string? RepeatType { get; set; }

        public DateTime CreatedDate { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public List<EventNotification>? EventNotifications { get; set; }
    }
}
