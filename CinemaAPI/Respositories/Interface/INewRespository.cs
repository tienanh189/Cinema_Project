using CinemaAPI.Models.Dto;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
namespace CinemaAPI.Respositories.Interface
{
    public interface INewRespository
    {
        public Task<IQueryable<NewDto>> GetAll();
        public Task<NewDto> GetById(Guid id);
        public Task<NewDto> Create(NewDto dto);
        public Task<NewDto> Update(Guid id, NewDto dto);
        public Task<bool> Delete(Guid id);
    }
}
