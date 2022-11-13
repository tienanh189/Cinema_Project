using Microsoft.AspNetCore.Identity;

namespace CinemaAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullNamme { get; set; }
        public bool IsDeleted { get; set; }
    }
}
