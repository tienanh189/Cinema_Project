using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Models
{
    public class CategoryMovie :BaseModel
    {
        public Guid CategoryMovieId { get; set; }
        public string? CategoryMovieName { get; set;}
        public ICollection<CategoryMovie_Movie> CategoryMovie_Movies { get; set; }
    }
}
