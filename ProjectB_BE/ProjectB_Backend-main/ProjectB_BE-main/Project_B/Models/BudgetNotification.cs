using System.ComponentModel.DataAnnotations;

namespace Project_B.Models
{
    public class BudgetNotification
    {
        [Key]
        public int NotificationID { get; set; }

        public int BudgetID { get; set; }

        [MaxLength(100)]
        public string NotificationType { get; set; }

        [MaxLength(255)]
        public string Message { get; set; }

        [MaxLength(255)]
        public string NotificationDate { get; set; }

        public Budget Budget { get; set; }
    }
}
