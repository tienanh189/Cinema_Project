using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Models.Dto
{
    public class CinemaDto : BaseModel
    {
        public Guid CinemaId { get; set; }
        public string? CinemaName { get; set; }
    }
}
