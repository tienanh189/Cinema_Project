using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Models
{
    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options)
        {
        }
    }
}
