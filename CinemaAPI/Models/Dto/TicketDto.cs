using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Models.Dto
{
    public class TicketDto : BaseModel
    {
        public Guid TicketId { get; set; }
        public Guid ShowTimeId { get; set; }
        public Guid BillId { get; set; }
        public Guid SeatId { get; set; }
        public float Price { get; set; }
    }

    public class CreateTicketDto
    {
        public Guid ShowTimeId { get; set; }
        public Guid BillId { get; set; }
        public List<SeatOnBillDto> ListSeat { get; set; }
    }
}
