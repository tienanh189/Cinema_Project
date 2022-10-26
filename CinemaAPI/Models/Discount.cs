namespace CinemaAPI.Models
{
    public class Discount : BaseModel
    {
        public Guid DiscountId { get; set; }
        public string? DiscountName { get; set; }
        public string? DiscountDetail { get; set; }
        public ICollection<Bill> Bills { get; set; }
    }
}
