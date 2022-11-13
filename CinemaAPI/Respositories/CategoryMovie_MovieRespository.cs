using AutoMapper;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CinemaAPI.Respositories
{
    public class CategoryMovie_MovieRespository : ICategoryMovie_MovieRespository
    {
        private readonly CinemaDbContext _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public CategoryMovie_MovieRespository(CinemaDbContext db, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        public async Task<CategoryMovie_MovieDto> Create(CategoryMovie_MovieDto dto)
        {
            Guid adminId;
            Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminId);
            var categoryM_M = _mapper.Map<CategoryMovie_Movie>(dto);
            categoryM_M.Id = Guid.NewGuid();
            categoryM_M.CreatedTime = DateTime.Now;
            categoryM_M.ModifiedTime = null;
            categoryM_M.DeletedTime = null;
            categoryM_M.CreatedByUser = adminId;
            categoryM_M.ModifiedByUser = null;
            categoryM_M.IsDeleted = false;
            _db.CategoryMovie_Movie.Add(categoryM_M);
            await _db.SaveChangesAsync();
            return _mapper.Map<CategoryMovie_MovieDto>(categoryM_M);
        }

        public async Task<bool> Delete(Guid id)
        {
            var categoryM_M = await _db.CategoryMovie_Movie.FindAsync(id);
            if (categoryM_M != null)
            {
                categoryM_M.IsDeleted = true;
                categoryM_M.DeletedTime = DateTime.Now;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IQueryable<CategoryMovie_MovieDto>> GetAll()
        {
            var categoryM_M = _db.CategoryMovie_Movie.Where(x => x.IsDeleted == false).AsEnumerable();
            return _mapper.Map<List<CategoryMovie_MovieDto>>(categoryM_M).AsQueryable();
        }

        public async Task<CategoryMovie_MovieDto> GetById(Guid id)
        {
            var categoryM_M = _db.CategoryMovie_Movie.Where(x => x.IsDeleted == false && x.Id == id).AsEnumerable();
            return _mapper.Map<CategoryMovie_MovieDto>(categoryM_M.FirstOrDefault());
        }

        public async Task<CategoryMovie_MovieDto> Update(Guid id, CategoryMovie_MovieDto dto)
        {
            Guid adminId;
            Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminId);
            var categoryM_M = await _db.CategoryMovie_Movie.FindAsync(id);
            if (categoryM_M != null)
            {
                categoryM_M.MovieId = dto.MovieId;
                categoryM_M.CategoryMovieId = dto.CategoryMovieId;
                categoryM_M.ModifiedTime = DateTime.Now;
                categoryM_M.ModifiedByUser = adminId;
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<CategoryMovie_MovieDto>(categoryM_M);
        }
    }
}
