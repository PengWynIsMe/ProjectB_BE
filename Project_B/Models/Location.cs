using System.ComponentModel.DataAnnotations;

namespace Project_B.Models
{
    public class Location
    {
        [Key]
        public int LocationID { get; set; }

        [MaxLength(100)]
        public string LocationName { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        public TimeSpan OpenTime { get; set; }

        [MaxLength(15)]
        public string Phone { get; set; }

        [MaxLength]
        public string Note { get; set; }

        public string Image { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
