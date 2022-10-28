using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Models.Dto
{
    public class CategoryMovie_MovieDto : BaseModel
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Guid CategoryMovieId { get; set; }
    }
}
