using CinemaAPI.Models.Dto;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories.Interface
{
    public interface IShowTime_SeatRespository
    {
        public Task<IQueryable<ShowTime_SeatDto>> GetAll();
        public Task<ShowTime_SeatDto> GetById(Guid id);
        public Task<ShowTime_SeatDto> Create(ShowTime_SeatDto dto);
        public Task<ShowTime_SeatDto> Update(Guid id, ShowTime_SeatDto dto);
        public Task<bool> Delete(Guid id);
    }
}
