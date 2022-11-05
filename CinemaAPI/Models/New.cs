namespace CinemaAPI.Models
{
    public class New : BaseModel
    {
        public Guid NewId { get; set; }
        public string? NewTittle { get; set; }
        public string? Description { get; set; }
        public string Image { get; set; }
    }
}
