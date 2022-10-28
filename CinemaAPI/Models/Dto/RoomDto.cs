using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
namespace CinemaAPI.Models.Dto
{
    public class RoomDto : BaseModel
    {
        public Guid RoomId { get; set; }
        public string? RoomName { get; set; }
        public Guid CinemaId { get; set; }
        public bool? Status { get; set; }
        public Cinema Cinemas { get; set; }
    }
}
