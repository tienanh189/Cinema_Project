using AutoMapper;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CinemaAPI.Respositories
{
    public class CategorySeatRespository : ICategorySeatRespository
    {
        private readonly CinemaDbContext _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public CategorySeatRespository(CinemaDbContext db, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        public async Task<CategorySeatDto> Create(CategorySeatDto dto)
        {
            Guid adminId;
            Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminId);
            var categorySeat = _mapper.Map<CategorySeat>(dto);
            categorySeat.CategorySeatId = Guid.NewGuid();
            categorySeat.CreatedTime = DateTime.Now;
            categorySeat.ModifiedTime = null;
            categorySeat.DeletedTime = null;
            categorySeat.CreatedByUser = adminId;
            categorySeat.ModifiedByUser = null;
            categorySeat.IsDeleted = false;
            _db.CategorySeat.Add(categorySeat);
            await _db.SaveChangesAsync();
            return _mapper.Map<CategorySeatDto>(categorySeat);
        }

        public async Task<bool> Delete(Guid id)
        {
            var categorySeat = await _db.CategorySeat.FindAsync(id);
            if (categorySeat != null)
            {
                categorySeat.IsDeleted = true;
                categorySeat.DeletedTime = DateTime.Now;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IQueryable<CategorySeatDto>> GetAll()
        {
            var categorySeat = _db.CategorySeat.Where(x => x.IsDeleted == false).AsEnumerable();
            return _mapper.Map<List<CategorySeatDto>>(categorySeat).AsQueryable();
        }

        public async Task<CategorySeatDto> GetById(Guid id)
        {
            var categorySeat = _db.CategorySeat.Where(x => x.IsDeleted == false && x.CategorySeatId == id).AsEnumerable();
            return _mapper.Map<CategorySeatDto>(categorySeat.FirstOrDefault());
        }

        public async Task<CategorySeatDto> Update(Guid id, CategorySeatDto dto)
        {
            Guid adminId;
            Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminId);
            var categorySeat = await _db.CategorySeat.FindAsync(id);
            if (categorySeat != null)
            {
                categorySeat.CategorySeatName = dto.CategorySeatName;
                categorySeat.Price= dto.Price;
                categorySeat.ModifiedTime = DateTime.Now;
                categorySeat.ModifiedByUser = adminId;
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<CategorySeatDto>(categorySeat);
        }
    }
}
