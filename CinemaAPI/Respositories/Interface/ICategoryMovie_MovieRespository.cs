using CinemaAPI.Models.Dto;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories.Interface
{
    public interface ICategoryMovie_MovieRespository
    {
        public Task<IQueryable<CategoryMovie_MovieDto>> GetAll();
        public Task<CategoryMovie_MovieDto> GetById(Guid id);
        public Task<CategoryMovie_MovieDto> Create(CategoryMovie_MovieDto dto);
        public Task<CategoryMovie_MovieDto> Update(Guid id, CategoryMovie_MovieDto dto);
        public Task<bool> Delete(Guid id);
    }
}
