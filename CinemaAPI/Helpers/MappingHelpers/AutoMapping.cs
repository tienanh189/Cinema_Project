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
            CreateMap<BillDto, Bill>();
            CreateMap<Bill, BillDto>();
            CreateMap<CategoryMovie_MovieDto, CategoryMovie_Movie>();
            CreateMap<CategoryMovie_Movie, CategoryMovie_MovieDto>();
            CreateMap<ShowTime_SeatDto, ShowTime_Seat>();
            CreateMap<ShowTime_Seat, ShowTime_SeatDto>();
        }
    }
}
