using CinemaAPI.Models.Dto;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories.Interface
{
    public interface IRoomRespository
    {
        public Task<IQueryable<RoomDto>> GetAll();
        public Task<RoomDto> GetById(Guid id);
        public Task<RoomDto> Create(RoomDto dto);
        public Task<RoomDto> Update(Guid id, RoomDto dto);
        public Task<bool> Delete(Guid id);
    }
}
