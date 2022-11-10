using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Models.Dto
{
    public class MovieDto : BaseModel
    {
        public Guid MovieId { get; set; }
        public string? MovieName { get; set; }
        public string? MovieDescription { get; set; }
        public int Duration { get; set; }
        public string? Actor { get; set; }
        public string? Director { get; set; }
        public string? Image { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime EndShowDate { get; set; }
        public bool? IsShowing { get; set; }
    }

    public class MovieDetail : MovieDto
    {  
        public MovieDetail(Guid movieId, string? movieName, string? movieDescription, int duration, string? actor, string? director, string? image, DateTime releaseDate,DateTime endDate, bool? isShowing) 
        {

            MovieId = movieId;
            MovieName = movieName;
            MovieDescription = movieDescription;
            Duration = duration;
            Actor = actor;
            Director = director;
            Image = image;
            ReleaseDate = releaseDate;
            EndShowDate = endDate;
            IsShowing = isShowing;
            this.CategoryMovies = new List<CategoryMovieDto>();
        }
        public List<CategoryMovieDto> CategoryMovies { get; set; }
    }
}
