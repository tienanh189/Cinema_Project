using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Models
{
    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options)
        {
        }

        public DbSet<Bill> Bill { get; set; }
        public DbSet<CategoryMovie> CategoryMovie { get; set; } 
        public DbSet<CategoryMovie_Movie> CategoryMovie_Movie { get; set; }
        public DbSet<CategorySeat> CategorySeat { get; set; }
        public DbSet<New> News { get; set; }
        public DbSet<Cinema> Cinema { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Seat> Seat { get; set; }
        public DbSet<ShowTime> ShowTime { get; set; }
        public DbSet<ShowTime_Seat> ShowTime_Seat { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserGroup> UserGroup {get; set; }
        public DbSet<UserGroup_Permission> UserGroup_Permission { get; set; }
        public DbSet<Shift> Shift { get; set; }
    }
}
