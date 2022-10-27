using CinemaAPI.Models.Dto;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories.Interface
{
    public interface ICategorySeatRespository
    {
        public Task<IQueryable<CategorySeatDto>> GetAll();
        public Task<CategorySeatDto> GetById(Guid id);
        public Task<CategorySeatDto> Create(CategorySeatDto dto);
        public Task<CategorySeatDto> Update(Guid id, CategorySeatDto dto);
        public Task<bool> Delete(Guid id);
    }
}
