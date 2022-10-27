using CinemaAPI.Models.Dto;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories.Interface
{
    public interface ICinemaRespository
    {
        public Task<IQueryable<CinemaDto>> GetAll();
        public Task<CinemaDto> GetById(Guid id);
        public Task<CinemaDto> Create(CinemaDto dto);
        public Task<CinemaDto> Update(Guid id, CinemaDto dto);
        public Task<bool> Delete(Guid id);
    }
}
