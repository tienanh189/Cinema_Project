using AutoMapper;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories
{
    public class RoomRespository : IRoomRespository
    {
        private readonly CinemaDbContext _db;
        private readonly IMapper _mapper;

        public RoomRespository(CinemaDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<RoomDto> Create(RoomDto dto)
        {
            var room = _mapper.Map<Room>(dto);
            room.RoomId = Guid.NewGuid();
            room.CreatedTime = DateTime.Now;
            room.ModifiedTime = null;
            room.DeletedTime = null;
            room.CreatedByUser = null;
            room.ModifiedByUser = null;
            room.IsDeleted = false;
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

        public async Task<RoomDto> Update(Guid id, RoomDto dto)
        {
            var room = await _db.Room.FindAsync(id);
            if (room != null)
            {
                room.RoomName = dto.RoomName;
                room.ModifiedTime = DateTime.Now;
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<RoomDto>(room);
        }
    }
}
