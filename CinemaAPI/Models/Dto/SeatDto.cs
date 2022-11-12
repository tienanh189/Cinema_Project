using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Models.Dto
{
    public class SeatDto : BaseModel
    {
        public Guid SeatId { get; set; }
        public string? SeatName { get; set; }
        public Guid RoomId { get; set; }
        public Guid CategorySeatId { get; set; }

    }

    public class DetailSeat : SeatDto
    {
        public DetailSeat()
        {

        }
        public float Price { get; set; }
        public bool IsSelected { get; set; }
    }

    public class SeatOnBillDto
    {
        public Guid SeatId { get;set; }
        public string? SeatName { get;set; }
        public float Price { get; set; }
    }
}
