using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Models
{
    public class CinemaDbContext : IdentityDbContext<ApplicationUser>
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options)
        {
        }

        public DbSet<Bill> Bill { get; set; }
        public DbSet<CategoryMovie> CategoryMovie { get; set; } 
        public DbSet<CategoryMovie_Movie> CategoryMovie_Movie { get; set; }
        public DbSet<CategorySeat> CategorySeat { get; set; }
        public DbSet<New> New { get; set; }
        public DbSet<Cinema> Cinema { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Seat> Seat { get; set; }
        public DbSet<ShowTime> ShowTime { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<Shift> Shift { get; set; }
    }
}
