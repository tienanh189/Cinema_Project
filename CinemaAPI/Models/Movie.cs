namespace CinemaAPI.Models
{
    public class Movie : BaseModel
    {
        public Guid MovieId { get; set; }
        public string? MovieName { get; set;}
        public string? MovieDescription { get; set;}
        public int? Duration { get; set;}
        public string? Actor { get; set; }
        public string? Director { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Image { get; set; }
        public ICollection<CategoryMovie_Movie> CategoryMovie_Movies { get; set; }
        public ICollection<ShowTime> ShowTimes { get; set; }
    }
}
