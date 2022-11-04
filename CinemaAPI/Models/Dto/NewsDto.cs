using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Models.Dto
{
    public class NewsDto : BaseModel
    {
        public Guid NewsId { get; set; }
        public string? NewsName { get; set; }
        public string? NewsDetail { get; set; }
        public string? Image { get; set; }
    }
}
