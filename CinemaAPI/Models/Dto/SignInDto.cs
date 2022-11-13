﻿using System.ComponentModel.DataAnnotations;

namespace CinemaAPI.Models.Dto
{
    public class SignInDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
