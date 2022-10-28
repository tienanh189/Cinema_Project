using AutoMapper;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;

namespace CinemaAPI.Helpers
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<CategoryMovie, CategoryMovieDto>();
            CreateMap<CategoryMovieDto, CategoryMovie>();


        }
    }
}
