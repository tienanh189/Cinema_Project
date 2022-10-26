namespace CinemaAPI.Models
{
    public class Seat : BaseModel
    {
        public Guid SeatId { get; set; }
        public string? SeatName { get; set; }
        public Guid CategorySeatId { get; set; }
        public CategorySeat CategorySeats { get; set; }
        public Guid RoomId { get; set; }
        public Room Rooms { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
