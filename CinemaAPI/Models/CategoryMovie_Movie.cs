namespace CinemaAPI.Models
{
    public class CategoryMovie_Movie : BaseModel
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Movie Movies { get; set; }
        public Guid CategoryMovieId { get; set; }
        public CategoryMovie CategoryMovies { get; set; }
    }
}
