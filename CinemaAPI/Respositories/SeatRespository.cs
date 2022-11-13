using AutoMapper;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CinemaAPI.Respositories
{
    public class SeatRespository : ISeatRespository
    {
        private readonly CinemaDbContext _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        public SeatRespository(CinemaDbContext db, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        public async Task<SeatDto> Create(SeatDto dto)
        {
            Guid adminID;
            Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminID);
            var seat = _mapper.Map<Seat>(dto);
            seat.SeatId = Guid.NewGuid();
            seat.CreatedTime = DateTime.Now;
            seat.ModifiedTime = null;
            seat.DeletedTime = null;
            seat.CreatedByUser = adminID;
            seat.ModifiedByUser = null;
            seat.IsDeleted = false;
            if (CheckIfSeatHasExist(seat))
            {
                seat.SeatId = Guid.Empty;
                return _mapper.Map<SeatDto>(seat);
            }
            _db.Seat.Add(seat);
            await _db.SaveChangesAsync();
            return _mapper.Map<SeatDto>(seat);
        }

        public async Task<bool> Delete(Guid id)
        {
            var seat = await _db.Seat.FindAsync(id);
            if (seat != null)
            {
                seat.IsDeleted = true;
                seat.DeletedTime = DateTime.Now;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IQueryable<SeatDto>> GetAll()
        {
            var seat = _db.Seat.Where(x => x.IsDeleted == false).OrderBy(x => x.SeatName).AsEnumerable();
            return _mapper.Map<List<SeatDto>>(seat).AsQueryable();
        }

        public async Task<IQueryable<SeatDto>> GetAllSeatInRoom(Guid id)
        {
            var seat = _db.Seat.Where(x => x.IsDeleted == false && x.RoomId == id).OrderBy(x => x.SeatName).AsEnumerable();
            return _mapper.Map<List<SeatDto>>(seat).AsQueryable();
        }

        public async Task<SeatDto> GetById(Guid id)
        {
            var seat = _db.Seat.Where(x => x.IsDeleted == false && x.SeatId == id).AsEnumerable();
            return _mapper.Map<SeatDto>(seat.FirstOrDefault());
        }

        public async Task<SeatDto> Update(Guid id, SeatDto dto)
        {
            var seat = await _db.Seat.FindAsync(id);
            if (seat != null)
            {
                Guid adminID;
                Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminID);
                seat.SeatName = dto.SeatName;
                seat.ModifiedTime = DateTime.Now;
                seat.ModifiedByUser = adminID;
                if (CheckIfSeatHasExist(seat))
                {
                    seat.SeatId = Guid.Empty;
                    return _mapper.Map<SeatDto>(seat);
                }
                else
                {
                    await _db.SaveChangesAsync();
                }
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<SeatDto>(seat);
        }

        bool CheckIfSeatHasExist(Seat seat)
        {
            var _seat = _db.Seat.Where(x => x.SeatName == seat.SeatName && x.RoomId == seat.RoomId).AsEnumerable();
            if (_seat != null)
            {
                return true;
            }
            return false;
        }
    }
}
