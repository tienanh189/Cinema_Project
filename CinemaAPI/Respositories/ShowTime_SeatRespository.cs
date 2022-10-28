using AutoMapper;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories
{
    public class ShowTime_SeatRespository : IShowTime_SeatRespository
    {
        private readonly CinemaDbContext _db;
        private readonly IMapper _mapper;

        public ShowTime_SeatRespository(CinemaDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ShowTime_SeatDto> Create(ShowTime_SeatDto dto)
        {
            var showTime_Seat = _mapper.Map<ShowTime_Seat>(dto);
            showTime_Seat.ShowTime_SeatId = Guid.NewGuid();
            showTime_Seat.CreatedTime = DateTime.Now;
            showTime_Seat.ModifiedTime = null;
            showTime_Seat.DeletedTime = null;
            showTime_Seat.CreatedByUser = null;
            showTime_Seat.ModifiedByUser = null;
            showTime_Seat.IsDeleted = false;
            _db.ShowTime_Seat.Add(showTime_Seat);
            await _db.SaveChangesAsync();
            return _mapper.Map<ShowTime_SeatDto>(showTime_Seat);
        }

        public async Task<bool> Delete(Guid id)
        {
            var showTime_Seat = await _db.ShowTime_Seat.FindAsync(id);
            if (showTime_Seat != null)
            {
                showTime_Seat.IsDeleted = true;
                showTime_Seat.DeletedTime = DateTime.Now;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IQueryable<ShowTime_SeatDto>> GetAll()
        {
            var showTime_Seat = _db.ShowTime_Seat.Where(x => x.IsDeleted == false).AsEnumerable();
            return _mapper.Map<List<ShowTime_SeatDto>>(showTime_Seat).AsQueryable();
        }

        public async Task<ShowTime_SeatDto> GetById(Guid id)
        {
            var showTime_Seat = _db.ShowTime_Seat.Where(x => x.IsDeleted == false && x.ShowTime_SeatId == id).AsEnumerable();
            return _mapper.Map<ShowTime_SeatDto>(showTime_Seat.FirstOrDefault());
        }

        public async Task<ShowTime_SeatDto> Update(Guid id, ShowTime_SeatDto dto)
        {
            var showTime_Seat = await _db.ShowTime_Seat.FindAsync(id);
            if (showTime_Seat != null)
            {
                showTime_Seat.ShowTimeId = dto.ShowTimeId;
                showTime_Seat.SeatId = dto.SeatId;
                showTime_Seat.ModifiedTime = DateTime.Now;
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<ShowTime_SeatDto>(showTime_Seat);
        }
    }
}
