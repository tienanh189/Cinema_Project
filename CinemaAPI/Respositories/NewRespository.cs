using AutoMapper;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CinemaAPI.Respositories
{
    public class NewRespository:INewRespository
    {
        private readonly CinemaDbContext _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public NewRespository(CinemaDbContext db, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }
        public async Task<NewDto> Create(NewDto dto)
        {
            Guid adminID;
            Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminID);
            var new_ = _mapper.Map<New>(dto);
            new_.NewId = Guid.NewGuid();
            new_.CreatedTime = DateTime.Now;
            new_.ModifiedTime = null;
            new_.DeletedTime = null;
            new_.CreatedByUser = adminID;
            new_.ModifiedByUser = null;
            new_.IsDeleted = false;
            _db.New.Add(new_);
            await _db.SaveChangesAsync();
            return _mapper.Map<NewDto>(new_);
        }

        public async Task<bool> Delete(Guid id)
        {
            var new_ = await _db.New.FindAsync(id);
            if (new_ != null)
            {
                new_.IsDeleted = true;
                new_.DeletedTime = DateTime.Now;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IQueryable<NewDto>> GetAll()
        {
            var news = _db.New.Where(x => x.IsDeleted == false).AsEnumerable();
            return _mapper.Map<List<NewDto>>(news).AsQueryable();
        }

        public async Task<NewDto> GetById(Guid id)
        {
            var new_ = _db.New.Where(x => x.IsDeleted == false && x.NewId == id).AsEnumerable();
            return _mapper.Map<NewDto>(new_.FirstOrDefault());
        }

        public async Task<NewDto> Update(Guid id, NewDto dto)
        {
            var new_ = await _db.New.FindAsync(id);
            if (new_ != null)
            {
                Guid adminID;
                Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminID);
                new_.NewTittle = dto.NewTittle;
                new_.Description = dto.Description;
                new_.Image = dto.Image;
                new_.ModifiedTime = DateTime.Now;
                new_.ModifiedByUser = adminID;
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<NewDto>(new_);
        }
    }
}
