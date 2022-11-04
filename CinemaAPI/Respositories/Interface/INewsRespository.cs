using CinemaAPI.Models.Dto;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories.Interface
{
    public interface INewsRespository
    {
        public Task<IQueryable<NewsDto>> GetAll();
        public Task<NewsDto> GetById(Guid id);
        public Task<NewsDto> Create(NewsDto dto);
        public Task<NewsDto> Update(Guid id, NewsDto dto);
        public Task<bool> Delete(Guid id);
    }
}
