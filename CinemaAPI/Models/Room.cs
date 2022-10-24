namespace CinemaAPI.Models
{
    public class Room : BaseModel
    {
        public Guid RoomId { get; set; }
        public string? RoomName { get; set; }
        public bool? Status { get; set; }
        public Guid CinemaId { get; set; }
        public Cinema Cinemas { get; set; }
        public ICollection<Seat> Seats { get; set; }
    }
}
