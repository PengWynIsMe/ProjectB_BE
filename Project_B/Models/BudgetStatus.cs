using System.ComponentModel.DataAnnotations;

namespace Project_B.Models
{
    public class BudgetStatus
    {
        [Key]
        public int StatusID { get; set; }

        [MaxLength]
        public string IconStatus { get; set; }

        [MaxLength(255)]
        public string Message { get; set; }
    }
}
