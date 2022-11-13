using AutoMapper;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CinemaAPI.Respositories
{
    public class CategoryMovieRespository : ICategoryMovieRespository
    {
        private readonly CinemaDbContext _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public CategoryMovieRespository(CinemaDbContext db, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }

        public async Task<CategoryMovieDto> Create(CategoryMovieDto dto)
        {
            Guid adminId;
            Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminId);
            var categoryMovie = _mapper.Map<CategoryMovie>(dto);
            categoryMovie.CategoryMovieId = Guid.NewGuid();
            categoryMovie.CreatedTime = DateTime.Now;
            categoryMovie.ModifiedTime = null;
            categoryMovie.DeletedTime = null;
            categoryMovie.CreatedByUser = adminId;
            categoryMovie.ModifiedByUser = null;
            categoryMovie.IsDeleted = false;

            if(CheckIfCatMovieHasExist(categoryMovie))
            {
                categoryMovie.CategoryMovieId = Guid.Empty;
                return _mapper.Map<CategoryMovieDto>(categoryMovie);
            }
            else
            {
                _db.CategoryMovie.Add(categoryMovie);
                await _db.SaveChangesAsync();
            }
            return _mapper.Map<CategoryMovieDto>(categoryMovie);
        }

        public async Task<bool> Delete(Guid id)
        {
            var categoryMovie = await _db.CategoryMovie.FindAsync(id);
            if (categoryMovie != null)
            {
                categoryMovie.IsDeleted = true;
                categoryMovie.DeletedTime = DateTime.Now;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IQueryable<CategoryMovieDto>> GetAll()
        {
            var categoryMovies = _db.CategoryMovie.Where(x => x.IsDeleted == false).AsEnumerable();
            return _mapper.Map<List<CategoryMovieDto>>(categoryMovies).AsQueryable();
        }

        public async Task<CategoryMovieDto> GetById(Guid id)
        {
            var categoryMovie = _db.CategoryMovie.Where(x => x.IsDeleted == false && x.CategoryMovieId == id).AsEnumerable();
            return _mapper.Map<CategoryMovieDto>(categoryMovie.FirstOrDefault());
        }

        public async Task<CategoryMovieDto> Update(Guid id, CategoryMovieDto dto)
        {
            Guid adminId;
            Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminId);
            var categoryMovie = await _db.CategoryMovie.FindAsync(id);
            if (categoryMovie != null)
            {
                categoryMovie.CategoryMovieName = dto.CategoryMovieName;
                categoryMovie.ModifiedTime = DateTime.Now;
                categoryMovie.ModifiedByUser = adminId;
                if(CheckIfCatMovieHasExist(categoryMovie))
                {
                    categoryMovie.CategoryMovieId = Guid.Empty;
                    return _mapper.Map<CategoryMovieDto>(categoryMovie);
                }
                else
                {
                    _db.CategoryMovie.Update(categoryMovie);
                    await _db.SaveChangesAsync();
                }
            }  
            return _mapper.Map<CategoryMovieDto>(categoryMovie);
        }


        bool CheckIfCatMovieHasExist(CategoryMovie categoryMovie)
        {
            var name = _db.CategoryMovie.Where(x => x.CategoryMovieName == categoryMovie.CategoryMovieName).AsEnumerable();
            if (name != null)
            {
                return true;
            }
            return false;
        }
    }
}
