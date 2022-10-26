namespace CinemaAPI.Models
{
    public class ShowTime : BaseModel
    {
        public Guid ShowTimeId { get; set; }
        public DateTime? ShowDate { get; set; }
        public DateTime? ShowTimeDetail {get; set; }
        public Guid MovieId { get; set; }
        public Movie Movies { get; set; }
        public Guid RoomId { get; set; }
        public Room Rooms { get; set; }
        public ICollection<ShowTime_Seat> ShowTime_Seats { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

    }
}
