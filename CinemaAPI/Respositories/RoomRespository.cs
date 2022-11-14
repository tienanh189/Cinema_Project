using AutoMapper;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CinemaAPI.Respositories
{
    public class RoomRespository : IRoomRespository
    {
        private readonly CinemaDbContext _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public RoomRespository(CinemaDbContext db, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        public async Task<RoomDto> Create(RoomDto dto)
        {
            Guid adminID;
            Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminID);
            var room = _mapper.Map<Room>(dto);
            room.RoomId = Guid.NewGuid();
            room.CreatedTime = DateTime.Now;
            room.ModifiedTime = null;
            room.DeletedTime = null;
            room.CreatedByUser = adminID;
            room.ModifiedByUser = null;
            room.IsDeleted = false;
            if (CheckIfRoomHasExist(room))
            {
                room.RoomId = Guid.Empty;
                return _mapper.Map<RoomDto>(room);
            }
            _db.Room.Add(room);
            await _db.SaveChangesAsync();
            return _mapper.Map<RoomDto>(room);
        }

        public async Task<bool> Delete(Guid id)
        {
            var room = await _db.Room.FindAsync(id);
            if (room != null)
            {
                room.IsDeleted = true;
                room.DeletedTime = DateTime.Now;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IQueryable<RoomDto>> GetAll()
        {
            var rooms = _db.Room.Where(x => x.IsDeleted == false).AsEnumerable();
            return _mapper.Map<List<RoomDto>>(rooms).AsQueryable();
        }

        public async Task<RoomDto> GetById(Guid id)
        {
            var room = _db.Room.Where(x => x.IsDeleted == false && x.RoomId == id).AsEnumerable();
            return _mapper.Map<RoomDto>(room.FirstOrDefault());
        }

        public async Task<IQueryable<RoomDto>> GetAllRoomInCinema(Guid id)
        {
            var rooms = _db.Room.Where(x => x.IsDeleted == false && x.CinemaId == id).AsEnumerable();
            return _mapper.Map<List<RoomDto>>(rooms).AsQueryable();
        }

        public async Task<RoomDto> Update(Guid id, RoomDto dto)
        {
            var room = await _db.Room.FindAsync(id);
            if (room != null)
            {
                Guid adminID;
                Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminID);
                room.RoomName = dto.RoomName;
                room.CinemaId = dto.CinemaId;
                room.Status = dto.Status;
                room.ModifiedTime = DateTime.Now;
                room.ModifiedByUser = adminID;
                if (CheckIfRoomHasExist(room))
                {
                    room.RoomId = Guid.Empty;
                    return _mapper.Map<RoomDto>(room);
                }
                else
                {
                    await _db.SaveChangesAsync();
                }
            }
            return _mapper.Map<RoomDto>(room);
        }

        bool CheckIfRoomHasExist(Room room)
        {
            var _room = _db.Room.Where(x => x.RoomName == room.RoomName).AsEnumerable();
            if (_room != null)
            {
                return true;
            }
            return false;
        }
    }
}
