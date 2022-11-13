using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.Models.Dto
{
    public class SignUpDto
    {
        [Required]
        public string? FullNamme { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConFirmPassword { get; set; }
        public List<string>? RoleName { get; set; }
 
    }
}
