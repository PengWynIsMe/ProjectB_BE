using System.ComponentModel.DataAnnotations;

namespace Project_B.Models
{
    public class Meal
    {
        [Key]
        public int MealID { get; set; }

        [MaxLength(100)]
        public required string MealName { get; set; }

        [MaxLength(100)]
        public string? Address { get; set; }
        public TimeSpan? CloseTime { get; set; }

        public TimeSpan? OpenTime { get; set; }

        [MaxLength(15)]
        public string? Phone { get; set; }

        [MaxLength]
        public string? Note { get; set; }

        public string? Image { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }
    }
}
