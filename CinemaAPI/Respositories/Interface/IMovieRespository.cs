using CinemaAPI.Models.Dto;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories.Interface
{
    public interface IMovieRespository
    {
        public Task<IQueryable<MovieDto>> GetAll();
        public Task<MovieDto> GetById(Guid id);
        public Task<MovieDto> Create(MovieDto dto);
        public Task<MovieDto> Update(Guid id, MovieDto dto);
        public Task<bool> Delete(Guid id);
    }
}
