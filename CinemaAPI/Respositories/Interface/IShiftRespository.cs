using CinemaAPI.Models.Dto;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories.Interface
{
    public interface IShiftRespository
    {
        public Task<IQueryable<ShiftDto>> GetAll();
        public Task<ShiftDto> GetById(Guid id);
        public Task<ShiftDto> Create(ShiftDto dto, Guid adminId);
        public Task<ShiftDto> Update(Guid id, ShiftDto dto, Guid adminId);
        public Task<bool> Delete(Guid id);
    }
}
