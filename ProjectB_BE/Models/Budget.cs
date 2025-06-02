using System.ComponentModel.DataAnnotations;

namespace Project_B.Models
{
    public class Budget
    {
        [Key]
        public int BudgetId { get; set; }

        [MaxLength(255)]
        public string BudgetName { get; set; }

        [MaxLength]
        public string IconImage { get; set; }

        public int AmountBudget { get; set; }

        public int Spent { get; set; }

        public int AmountLeft { get; set; }

        public int DaysLeft { get; set; }

        public int RecommendedDailySpending { get; set; }

        public int ActualDailySpending { get; set; }

        public int TodaySpending { get; set; }

        public DateTime DateStart { get; set; }

        public int NotificationID { get; set; }

        public BudgetNotification BudgetNotification { get; set; }

        public DateTime DateEnd { get; set; }

        public int StatusID { get; set; }

        [MaxLength]
        public string Notes { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UserID { get; set; }

        public User User { get; set; }

        public List<Transaction> Transactions { get; set; }
    }
}
