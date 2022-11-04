namespace CinemaAPI.Models
{
    public class Shift :BaseModel
    {
        public Guid ShiftId { get; set; }
        public DateTime StartShift { get; set; }
        public DateTime EndShift { get; set; } 
        public ICollection<ShowTime> Showtimes { get; set; }
    }
}
