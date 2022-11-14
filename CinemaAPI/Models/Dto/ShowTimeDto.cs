using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Models.Dto
{
    public class ShowTimeDto
    {
        public Guid ShowTimeId { get; set; }
        public DateTime? ShowDate { get; set; }
        public Guid ShiftId { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public Guid MovieId { get; set; }
        public string? MovieName { get; set; }
        public Guid RoomId { get; set; }
        public int Duration { get; set; }
        public string? RoomName { get; set; }

    }
}
