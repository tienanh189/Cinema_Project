namespace CinemaAPI.Models
{
    public class ShowTime_Seat : BaseModel
    {
        public Guid ShowTime_SeatId { get; set; }
        public Guid ShowTimeId { get; set; }
        public ShowTime ShowTimes { get; set; }
        public Guid SeatId { get; set; }
        public Seat Seats { get; set; }
    }
}
