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
        }
    }
}
