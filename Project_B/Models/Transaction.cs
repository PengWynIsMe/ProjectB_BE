using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_B.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }

        public int TransactionType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TransactionDate { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [MaxLength(50)]
        public string Category { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BalanceAfterTransaction { get; set; }

        public int BudgetID { get; set; }

        [ForeignKey("BudgetID")]
        public Budget Budget { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
