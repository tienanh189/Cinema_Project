using CinemaAPI.Models.Dto;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories.Interface
{
    public interface IShowTimeRespository
    {
        public Task<IQueryable<ShowTimeDto>> GetAll();
        public Task<ShowTimeDto> GetById(Guid id);
        public Task<ShowTimeDto> Create(ShowTimeDto dto);
        public Task<ShowTimeDto> Update(Guid id, ShowTimeDto dto);
        public Task<bool> Delete(Guid id);
    }
}
