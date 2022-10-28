using CinemaAPI.Models.Dto;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories.Interface
{
    public interface IDiscountRespository
    {
        public Task<IQueryable<DiscountDto>> GetAll();
        public Task<DiscountDto> GetById(Guid id);
        public Task<DiscountDto> Create(DiscountDto dto);
        public Task<DiscountDto> Update(Guid id, DiscountDto dto);
        public Task<bool> Delete(Guid id);
    }
}
