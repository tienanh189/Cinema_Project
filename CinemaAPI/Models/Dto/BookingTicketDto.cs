namespace CinemaAPI.Models.Dto
{
    public class BookingTicketDto
    {
        public Guid CinemaId { get; set; }
        public DateTime Date { get; set; }
        public Guid ShowTimeId { get; set; }
        public List<SeatOnBillDto> ListSeat { get; set; }
    }
}
