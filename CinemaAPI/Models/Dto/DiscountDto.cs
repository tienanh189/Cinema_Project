using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Models.Dto
{
    public class DiscountDto : BaseModel
    {
        public Guid DiscountId { get; set; }
        public string? DiscountName { get; set; }
        public string? DiscountDetail { get; set; }
    }
}
