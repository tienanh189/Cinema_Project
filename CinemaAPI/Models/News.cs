namespace CinemaAPI.Models
{
    public class News : BaseModel
    {
        public Guid NewsId { get; set; }
        public string? NewsName { get; set; }
        public string? NewsDetail { get; set; }
        public string? Image { get; set; }
    }
}
