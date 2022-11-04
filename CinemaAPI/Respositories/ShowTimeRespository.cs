using AutoMapper;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories
{
    public class ShowTimeRespository : IShowTimeRespository
    {
        private readonly CinemaDbContext _db;
        private readonly IMapper _mapper;

        public ShowTimeRespository(CinemaDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<ShowTimeDto> Create(ShowTimeDto dto)
        {
            var showTime = _mapper.Map<ShowTime>(dto);
            showTime.ShowTimeId = Guid.NewGuid();
            showTime.CreatedTime = DateTime.Now;
            showTime.ModifiedTime = null;
            showTime.DeletedTime = null;
            showTime.CreatedByUser = null;
            showTime.ModifiedByUser = null;
            showTime.IsDeleted = false;
            _db.ShowTime.Add(showTime);
            await _db.SaveChangesAsync();
            return _mapper.Map<ShowTimeDto>(showTime);
        }

        public async Task<bool> Delete(Guid id)
        {
            var showTime = await _db.ShowTime.FindAsync(id);
            if (showTime != null)
            {
                showTime.IsDeleted = true;
                showTime.DeletedTime = DateTime.Now;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IQueryable<ShowTimeDto>> GetAll()
        {
            var showTimes = _db.ShowTime.Where(x => x.IsDeleted == false).AsEnumerable();
            return _mapper.Map<List<ShowTimeDto>>(showTimes).AsQueryable();
        }

        public async Task<ShowTimeDto> GetById(Guid id)
        {
            var showTime = _db.ShowTime.Where(x => x.IsDeleted == false && x.ShowTimeId == id).AsEnumerable();
            return _mapper.Map<ShowTimeDto>(showTime.FirstOrDefault());
        }

        public async Task<ShowTimeDto> Update(Guid id, ShowTimeDto dto)
        {
            var showTime = await _db.ShowTime.FindAsync(id);
            if (showTime != null)
            {
                showTime.ShowDate = dto.ShowDate;
                showTime.MovieId = dto.MovieId;
                showTime.RoomId = dto.RoomId;
                showTime.Rooms = dto.Rooms;
                showTime.Movies = dto.Movies;
                showTime.ModifiedTime = DateTime.Now;
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<ShowTimeDto>(showTime);
        }

        public async Task<ShowTimeDto> CreateAndCheck(ShowTimeDto dto)
        {
            var showTime = _mapper.Map<ShowTime>(dto);
            showTime.ShowTimeId = Guid.NewGuid();
            showTime.CreatedTime = DateTime.Now;
            showTime.ModifiedTime = null;
            showTime.DeletedTime = null;
            showTime.CreatedByUser = null;
            showTime.ModifiedByUser = null;
            showTime.IsDeleted = false;

            var check = _db.ShowTime.Where(x => x.MovieId == showTime.MovieId && x.RoomId == showTime.RoomId).ToList();
            if(check != null)
            {
                _db.ShowTime.Add(showTime);
                await _db.SaveChangesAsync();
            }
            else
            {

            }
            return _mapper.Map<ShowTimeDto>(showTime);
        }
    }
}
