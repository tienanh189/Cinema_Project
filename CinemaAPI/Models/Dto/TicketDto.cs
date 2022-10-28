using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Models.Dto
{
    public class TicketDto
    {
        public Guid TicketId { get; set; }
        public Guid ShowTimeId { get; set; }
        public ShowTime ShowTimes { get; set; }
        public Guid BillId { get; set; }
        public Bill Bills { get; set; }
        public Guid SeatId { get; set; }
        public Seat Seats { get; set; }
        public float Price { get; set; }
    }
}
