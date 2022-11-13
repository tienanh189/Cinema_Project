using AutoMapper;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories
{
    public class TicketRespository : ITicketRespository
    {
        private readonly CinemaDbContext _db;
        private readonly IMapper _mapper;

        public TicketRespository(CinemaDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<TicketDto> Create(TicketDto dto, Guid userID)
        {
            var ticket = _mapper.Map<Ticket>(dto);
            ticket.TicketId = Guid.NewGuid();
            ticket.CreatedTime = DateTime.Now;
            ticket.ModifiedTime = null;
            ticket.DeletedTime = null;
            ticket.CreatedByUser = userID;
            ticket.ModifiedByUser = null;
            ticket.IsDeleted = false;
            _db.Ticket.Add(ticket);
            await _db.SaveChangesAsync();
            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task<bool> Delete(Guid id)
        {
            var ticket = await _db.Ticket.FindAsync(id);
            if (ticket != null)
            {
                ticket.IsDeleted = true;
                ticket.DeletedTime = DateTime.Now;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IQueryable<TicketDto>> GetAll()
        {
            var tickets = _db.Ticket.Where(x => x.IsDeleted == false).AsEnumerable();
            return _mapper.Map<List<TicketDto>>(tickets).AsQueryable();
        }

        public async Task<TicketDto> GetById(Guid id)
        {
            var ticket = _db.Ticket.Where(x => x.IsDeleted == false && x.TicketId == id).AsEnumerable();
            return _mapper.Map<TicketDto>(ticket.FirstOrDefault());
        }

        public async Task<TicketDto> Update(Guid id, TicketDto dto, Guid adminId)
        {
            var ticket = await _db.Ticket.FindAsync(id);
            if (ticket != null)
            {
                ticket.ShowTimeId = dto.ShowTimeId;
                ticket.BillId = dto.BillId;
                ticket.SeatId = dto.SeatId;
                ticket.Price = dto.Price;
                ticket.ModifiedByUser = adminId;
                ticket.ModifiedTime = DateTime.Now;
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task<IQueryable<TicketDto>> GetMyTicket(Guid id)
        {
            var tickets = _db.Ticket.Where(x => x.IsDeleted == false && x.CreatedByUser == id).AsEnumerable();

            return _mapper.Map<List<TicketDto>>(tickets).AsQueryable();
        }
    }
}
