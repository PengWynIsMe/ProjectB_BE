using System.ComponentModel.DataAnnotations;

namespace Project_B.DTOs
{
    public class UserStatusUpdateDTO
    {
        public int UserId { get; set; }
        public int Status { get; set; }
    }
}
