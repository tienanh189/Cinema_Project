namespace CinemaAPI.Models
{
    public class Cinema : BaseModel
    {
        public Guid CinemaId { get; set; }
        public string? CinemaName { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}
