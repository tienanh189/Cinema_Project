using AutoMapper;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories
{
    public class CinemaRespository :ICinemaRespository
    {
        private readonly CinemaDbContext _db;
        private readonly IMapper _mapper;

        public CinemaRespository(CinemaDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<CinemaDto> Create(CinemaDto dto)
        {
            var cinema = _mapper.Map<Cinema>(dto);
            cinema.CinemaId = Guid.NewGuid();
            cinema.CreatedTime = DateTime.Now;
            cinema.ModifiedTime = null;
            cinema.DeletedTime = null;
            cinema.CreatedByUser = null;
            cinema.ModifiedByUser = null;
            cinema.IsDeleted = false;
            _db.Cinema.Add(cinema);
            await _db.SaveChangesAsync();
            return _mapper.Map<CinemaDto>(cinema);
        }

        public async Task<bool> Delete(Guid id)
        {
            var cinema = await _db.Cinema.FindAsync(id);
            if (cinema != null)
            {
                cinema.IsDeleted = true;
                cinema.DeletedTime = DateTime.Now;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IQueryable<CinemaDto>> GetAll()
        {
            var cinemas = _db.Cinema.Where(x => x.IsDeleted == false).AsEnumerable();
            return _mapper.Map<List<CinemaDto>>(cinemas).AsQueryable();
        }

        public async Task<CinemaDto> GetById(Guid id)
        {
            var cinema = _db.Cinema.Where(x => x.IsDeleted == false && x.CinemaId == id).AsEnumerable();
            return _mapper.Map<CinemaDto>(cinema.FirstOrDefault());
        }

        public async Task<CinemaDto> Update(Guid id, CinemaDto dto)
        {
            var cinema = await _db.Cinema.FindAsync(id);
            if (cinema != null)
            {
                cinema.CinemaName = dto.CinemaName;
                cinema.ModifiedTime = DateTime.Now;
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<CinemaDto>(cinema);
        }
    }
}
