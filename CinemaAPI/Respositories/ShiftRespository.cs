using AutoMapper;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CinemaAPI.Respositories
{
    public class ShiftRespository : IShiftRespository
    {
        private readonly CinemaDbContext _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public ShiftRespository(CinemaDbContext db, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        public async Task<ShiftDto> Create(ShiftDto dto)
        {
            Guid adminID;
            Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminID);
            var shift = _mapper.Map<Shift>(dto);
            shift.ShiftId = Guid.NewGuid();
            shift.CreatedTime = DateTime.Now;
            shift.ModifiedTime = null;
            shift.DeletedTime = null;
            shift.CreatedByUser = adminID;
            shift.ModifiedByUser = null;
            shift.IsDeleted = false;

            if (CheckIfTimeHasExist(shift))
            {
                shift.ShiftId = Guid.Empty;
                return _mapper.Map<ShiftDto>(shift);
            }
            if (ReturnTime(shift.StartTime) < ReturnTime(shift.EndTime))
            {
                _db.Shift.Add(shift);
                await _db.SaveChangesAsync();
            }
            else
            {
                shift.ShiftId = Guid.Empty;
            }
            return _mapper.Map<ShiftDto>(shift);
        }

        public async Task<bool> Delete(Guid id)
        {
            var shift = await _db.Shift.FindAsync(id);
            if (shift != null)
            {
                shift.IsDeleted = true;
                shift.DeletedTime = DateTime.Now;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IQueryable<ShiftDto>> GetAll()
        {
            var shift = _db.Shift.Where(x => x.IsDeleted == false).AsEnumerable();
            return _mapper.Map<List<ShiftDto>>(shift).AsQueryable();
        }

        public async Task<ShiftDto> GetById(Guid id)
        {
            var shift = _db.Shift.Where(x => x.IsDeleted == false && x.ShiftId == id).AsEnumerable();
            return _mapper.Map<ShiftDto>(shift.FirstOrDefault());
        }

        public async Task<ShiftDto> Update(Guid id, ShiftDto dto)
        {
            var shift = await _db.Shift.FindAsync(id);
            if (shift != null)
            {
                Guid adminID;
                Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminID);
                shift.StartTime = dto.StartTime;
                shift.EndTime = dto.EndTime;
                shift.ModifiedTime = DateTime.Now;
                shift.ModifiedByUser = adminID;
                if (CheckIfTimeHasExist(shift))
                {
                    shift.ShiftId = Guid.Empty;
                    return _mapper.Map<ShiftDto>(shift);
                }
                if (ReturnTime(shift.StartTime) < ReturnTime(shift.EndTime))
                {
                    await _db.SaveChangesAsync();
                }
                else
                {
                    shift.ShiftId = Guid.Empty;
                }
            }
            //await _db.SaveChangesAsync();
            return _mapper.Map<ShiftDto>(shift);
        }

        private int ReturnTime(string time)
        {
            string t = time.Substring(0, time.LastIndexOf(":"));
            int hour = int.Parse(t);
            return hour;   
        }

        bool CheckIfTimeHasExist(Shift shift)
        {
            var time = _db.Shift.Where(x => x.StartTime == shift.StartTime && x.EndTime == shift.EndTime).FirstOrDefault();
            if (time != null)
            {
                return true;
            }
            return false;
        }
    }
}
