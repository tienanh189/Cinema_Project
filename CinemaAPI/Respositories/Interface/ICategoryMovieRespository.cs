using CinemaAPI.Models.Dto;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories.Interface
{
    public interface ICategoryMovieRespository
    {
        public Task<IQueryable<CategoryMovieDto>> GetAll();
        public Task<CategoryMovieDto> GetById(Guid id);
        public Task<CategoryMovieDto> Create(CategoryMovieDto dto);
        public Task<CategoryMovieDto> Update(Guid id, CategoryMovieDto dto);
        public Task<bool> Delete(Guid id);
    }
}
