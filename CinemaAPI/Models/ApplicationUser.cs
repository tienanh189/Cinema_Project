using Microsoft.AspNetCore.Identity;

namespace CinemaAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullNamme { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<Bill> bills { get; set; }
        public ICollection<CategoryMovie> categoryMovies { get; set; }
        public ICollection<CategorySeat> categorySeats { get; set; }
        public ICollection<CategoryMovie_Movie> categoryMovie_Movies { get; set; }
        public ICollection<Movie> movies { get; set; }
        public ICollection<Seat> seats { get; set; }
        public ICollection<ShowTime> showTimes { get; set; }
        public ICollection<Ticket> tickets { get; set; }
        public ICollection<Cinema> cinemas { get; set; }
        public ICollection<New> news { get; set; }
        public ICollection<Room> rooms { get; set; }   
        public ICollection<Shift> shifts { get; set; }
    }
}
