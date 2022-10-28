using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Models.Dto
{
    public class ShowTimeDto
    {
        public Guid ShowTimeId { get; set; }
        public DateTime? ShowDate { get; set; }
        public DateTime? ShowTimeDetail { get; set; }
        public Guid MovieId { get; set; }
        public Guid RoomId { get; set; }
        public Movie Movies { get; set; }
        public Room Rooms { get; set; }
    }
}
