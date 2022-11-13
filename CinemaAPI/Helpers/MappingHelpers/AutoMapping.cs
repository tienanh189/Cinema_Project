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
            CreateMap<BillDto, Bill>();
            CreateMap<Bill, BillDto>();
            CreateMap<CategoryMovie_MovieDto, CategoryMovie_Movie>();
            CreateMap<CategoryMovie_Movie, CategoryMovie_MovieDto>();
            CreateMap<Room, RoomDto>();
            CreateMap<RoomDto, Room>();
            CreateMap<Cinema, CinemaDto>();
            CreateMap<CinemaDto, Cinema>();
            CreateMap<Movie, MovieDto>();
            CreateMap<MovieDto, Movie>();
            CreateMap<ShowTime, ShowTimeDto>();
            CreateMap<ShowTimeDto, ShowTime>();
            CreateMap<Ticket, TicketDto>();
            CreateMap<TicketDto, Ticket>();
            CreateMap<Shift, ShiftDto>();
            CreateMap<ShiftDto, Shift>();
            CreateMap<New, NewDto>();
            CreateMap<NewDto, New>();
        }
    }
}
