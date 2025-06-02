using System.ComponentModel.DataAnnotations;

namespace Project_B.Models
{
    public class EventNotification
    {
        [Key]
        public int NotificationID { get; set; }

        public int EventID { get; set; }

        [MaxLength(255)]
        public string Message { get; set; }

        public DateTime NotificationTime { get; set; }

        public DateTime NotificationDate { get; set; }

        public int Status { get; set; }

        public Event Event { get; set; }
    }
}
