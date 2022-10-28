using CinemaAPI.Models.Dto;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories.Interface
{
    public interface ITicketRespository
    {
        public Task<IQueryable<TicketDto>> GetAll();
        public Task<TicketDto> GetById(Guid id);
        public Task<TicketDto> Create(TicketDto dto);
        public Task<TicketDto> Update(Guid id, TicketDto dto);
        public Task<bool> Delete(Guid id);
    }
}
