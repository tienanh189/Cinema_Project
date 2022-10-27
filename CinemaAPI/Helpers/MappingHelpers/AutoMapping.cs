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
            CreateMap<SeatDto, Seat>();
            CreateMap<Seat, SeatDto>();
            CreateMap<CategorySeatDto, CategorySeat>();
            CreateMap<CategorySeat, CategorySeatDto>();
            CreateMap<DiscountDto, Discount>();
            CreateMap<Discount, DiscountDto>();
        }
    }
}
