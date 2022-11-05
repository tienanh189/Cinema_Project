namespace CinemaAPI.Models
{
    public class Shift : BaseModel
    {
        public Guid ShiftId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public ICollection<ShowTime> ShowTimes { get; set; }
    }
}
