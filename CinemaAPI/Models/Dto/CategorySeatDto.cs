using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Models.Dto
{
    public class CategorySeatDto : BaseModel
    {
        public Guid CategorySeatId { get; set; }
        public string? CategorySeatName { get; set; }
        public float Price { get; set; }

    }
}
