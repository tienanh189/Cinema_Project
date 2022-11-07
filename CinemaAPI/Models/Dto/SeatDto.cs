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
        public bool IsSelected { get; set; }
    }
}
