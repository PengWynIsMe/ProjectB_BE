using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_B.Models
{
    public class Goal
    {
        [Key]
        public int GoalID { get; set; }

        [MaxLength(100)]
        public string GoalName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountTarget { get; set; }

        public int AmountSaved { get; set; }

        public int DaysLeft { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        [MaxLength(255)]
        public string Notes { get; set; }

        public DateTime CreatedDate { get; set; }

        public int Status { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
