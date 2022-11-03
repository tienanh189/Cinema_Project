using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Models.Dto
{
    public class MovieDto : BaseModel
    {
        public Guid MovieId { get; set; }
        public string? MovieName { get; set; }
        public string? MovieDescription { get; set; }
        public int? Duration { get; set; }
        public string? Actor { get; set; }
        public string? Director { get; set; }
        public string? Image { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool? IsShowing { get; set; }
    }

    public class MovieDetail : MovieDto
    {
        public MovieDetail()
        {
            ListCategoryMovieName = new List<string>();
        }

        public List<string> ListCategoryMovieName { get; set; }
    }
}
