﻿using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Models.Dto
{
    public class MovieDto : BaseModel
    {
        public MovieDto(Guid movieId, string? movieName, string? movieDescription, int? duration, string? actor, string? director, string? image, DateTime releaseDate, bool? isShowing)
        {
            MovieId = movieId;
            MovieName = movieName;
            MovieDescription = movieDescription;
            Duration = duration;
            Actor = actor;
            Director = director;
            Image = image;
            ReleaseDate = releaseDate;
            IsShowing = isShowing;
        }

        public Guid MovieId { get; set; }
        public string? MovieName { get; set; }
        public string? MovieDescription { get; set; }
        public int? Duration { get; set; }
        public string? Actor { get; set; }
        public string? Director { get; set; }
        public string? Image { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool? IsShowing { get; set; }
    }

    public class MovieDetail : MovieDto
    {  
        public MovieDetail(Guid movieId, string? movieName, string? movieDescription, int? duration, string? actor, string? director, string? image, DateTime releaseDate, bool? isShowing) 
            : base(movieId, movieName, movieDescription, duration, actor, director, image,releaseDate, isShowing)
        {
            this.ListCategoryMovieName = new List<string>();
        }
        public List<string> ListCategoryMovieName { get; set; }
    }
}
