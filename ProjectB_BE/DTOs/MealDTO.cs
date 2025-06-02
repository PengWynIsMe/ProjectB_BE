namespace Project_B.DTOs
{
    public class MealDTO
    {
        public int? MealID { get; set; }
        public string MealName { get; set; }
        public string Address { get; set; }
        public TimeSpan? OpenTime { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        public int UserId { get; set; }
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
