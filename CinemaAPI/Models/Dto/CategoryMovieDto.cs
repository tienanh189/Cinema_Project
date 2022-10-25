namespace CinemaAPI.Models.Dto
{
    public class CategoryMovieDto : BaseModel
    {
        public Guid CategoryMovieId { get; set; }
        public string? CategoryMovieName { get; set; }
    }
}
