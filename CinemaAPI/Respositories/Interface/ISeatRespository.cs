using CinemaAPI.Models.Dto;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories.Interface
{
    public interface ISeatRespository
    {
        public Task<IQueryable<SeatDto>> GetAll();
        public Task<SeatDto> GetById(Guid id);
        public Task<SeatDto> Create(SeatDto dto);
        public Task<SeatDto> Update(Guid id, SeatDto dto);
        public Task<bool> Delete(Guid id);
    }
}
