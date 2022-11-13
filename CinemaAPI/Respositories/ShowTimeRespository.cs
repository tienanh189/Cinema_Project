using AutoMapper;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CinemaAPI.Respositories
{
    public class ShowTimeRespository : IShowTimeRespository
    {
        private readonly CinemaDbContext _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public ShowTimeRespository(CinemaDbContext db, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }
        public async Task<ShowTimeDto> Create(ShowTimeDto dto)
        {
            Guid adminID;
            Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminID);
            var showTime = _mapper.Map<ShowTime>(dto);
            showTime.ShowTimeId = Guid.NewGuid();
            showTime.CreatedTime = DateTime.Now;
            showTime.ModifiedTime = null;
            showTime.DeletedTime = null;
            showTime.CreatedByUser = adminID;
            showTime.ModifiedByUser = null;
            showTime.IsDeleted = false;
            if (CheckShowTimeHasExist(showTime))
            {
                showTime.ShowTimeId = Guid.Empty;
            }
            else
            {
                _db.ShowTime.Add(showTime);
                await _db.SaveChangesAsync();                
            }
            return _mapper.Map<ShowTimeDto>(showTime);
        }

        public Task<ShowTimeDto> CreateAndCheck(ShowTimeDto dto)
        {
            throw new NotImplementedException();
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
                Guid adminID;
                Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminID);
                showTime.ShowDate = dto.ShowDate;
                showTime.MovieId = dto.MovieId;
                showTime.RoomId = dto.RoomId;
                showTime.ModifiedTime = DateTime.Now;
                showTime.ModifiedByUser = adminID;
                if (CheckShowTimeHasExist(showTime))
                {
                    showTime.ShowTimeId = Guid.Empty;
                }
                else
                {
                    await _db.SaveChangesAsync();
                }                
            }        
            return _mapper.Map<ShowTimeDto>(showTime);
        }

        
        bool CheckShowTimeHasExist(ShowTime showTime)
        {
            var result=_db.ShowTime.Where(x=>x.ShowDate==showTime.ShowDate && 
            x.ShiftId==showTime.ShiftId && x.RoomId==showTime.RoomId).FirstOrDefault();
            if (result != null)
            {                
                return true;
            }
            return false;
            
        }
    }
}
