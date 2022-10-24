namespace CinemaAPI.Models
{
    public class CategorySeat : BaseModel
    {
        public Guid CategorySeatId { get; set; }
        public string? CategorySeatName { get; set; }
        public ICollection<Seat> Seats { get; set; }
    }
}
