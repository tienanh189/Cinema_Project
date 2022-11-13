using AutoMapper;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CinemaAPI.Respositories
{
    public class TicketRespository : ITicketRespository
    {
        private readonly CinemaDbContext _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public TicketRespository(CinemaDbContext db, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }
        public async Task<TicketDto> Create(TicketDto dto)
        {
            Guid adminID;
            Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminID);
            var ticket = _mapper.Map<Ticket>(dto);
            ticket.TicketId = Guid.NewGuid();
            ticket.CreatedTime = DateTime.Now;
            ticket.ModifiedTime = null;
            ticket.DeletedTime = null;
            ticket.CreatedByUser = adminID;
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

        public async Task<TicketDto> Update(Guid id, TicketDto dto)
        {
            var ticket = await _db.Ticket.FindAsync(id);
            if (ticket != null)
            {
                Guid adminID;
                Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminID);
                ticket.ShowTimeId = dto.ShowTimeId;
                ticket.BillId = dto.BillId;
                ticket.SeatId = dto.SeatId;
                ticket.Price = dto.Price;
                ticket.ModifiedTime = DateTime.Now;
                ticket.ModifiedByUser = adminID;
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task<IQueryable<TicketDto>> GetMyTicket()
        {
            Guid adminID;
            Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminID);
            var tickets = _db.Ticket.Where(x => x.IsDeleted == false && x.CreatedByUser == adminID ).AsEnumerable();

            return _mapper.Map<List<TicketDto>>(tickets).AsQueryable();
        }
    }
}
