using AutoMapper;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories
{
    public class SeatRespository : ISeatRespository
    {
        private readonly CinemaDbContext _db;
        private readonly IMapper _mapper;

        public SeatRespository(CinemaDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<SeatDto> Create(SeatDto dto)
        {
            var seat = _mapper.Map<Seat>(dto);
            seat.SeatId = Guid.NewGuid();
            seat.CreatedTime = DateTime.Now;
            seat.ModifiedTime = null;
            seat.DeletedTime = null;
            seat.CreatedByUser = null;
            seat.ModifiedByUser = null;
            seat.IsDeleted = false;
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
                seat.SeatName = dto.SeatName;
                seat.ModifiedTime = DateTime.Now;
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<SeatDto>(seat);
        }
    }
}
