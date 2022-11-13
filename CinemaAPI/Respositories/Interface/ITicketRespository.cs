using CinemaAPI.Models.Dto;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories.Interface
{
    public interface ITicketRespository
    {
        public Task<IQueryable<TicketDto>> GetAll();
        public Task<TicketDto> GetById(Guid id);
        public Task<TicketDto> Create(TicketDto dto, Guid userID);
        public Task<TicketDto> Update(Guid id, TicketDto dto, Guid adminId);
        public Task<bool> Delete(Guid id);

        public Task<IQueryable<TicketDto>> GetMyTicket(Guid id);
    }
}
