using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Models.Dto
{
    public class ShowTime_SeatDto : BaseModel
    {
        public Guid ShowTime_SeatId { get; set; }
        public Guid ShowTimeId { get; set; }
        public Guid SeatId { get; set; }
    }
}
