using AutoMapper;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories
{
    public class NewsRespository : INewsRespository 
    {
        private readonly CinemaDbContext _db;
        private readonly IMapper _mapper;

        public NewsRespository(CinemaDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<NewsDto> Create(NewsDto dto)
        {
            var news = _mapper.Map<News>(dto);
            news.NewsId = Guid.NewGuid();
            news.CreatedTime = DateTime.Now;
            news.ModifiedTime = null;
            news.DeletedTime = null;
            news.CreatedByUser = null;
            news.ModifiedByUser = null;
            news.IsDeleted = false;
            _db.News.Add(news);
            await _db.SaveChangesAsync();
            return _mapper.Map<NewsDto>(news);
        }

        public async Task<bool> Delete(Guid id)
        {
            var news = await _db.News.FindAsync(id);
            if (news != null)
            {
                news.IsDeleted = true;
                news.DeletedTime = DateTime.Now;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IQueryable<NewsDto>> GetAll()
        {
            var newsS = _db.News.Where(x => x.IsDeleted == false).AsEnumerable();
            return _mapper.Map<List<NewsDto>>(newsS).AsQueryable();
        }

        public async Task<NewsDto> GetById(Guid id)
        {
            var news = _db.News.Where(x => x.IsDeleted == false && x.NewsId == id).AsEnumerable();
            return _mapper.Map<NewsDto>(news.FirstOrDefault());
        }

        public async Task<NewsDto> Update(Guid id, NewsDto dto)
        {
            var news = await _db.News.FindAsync(id);
            if (news != null)
            {
                news.NewsName = dto.NewsName;
                news.NewsDetail = dto.NewsDetail;
                news.Image = dto.Image;
                news.ModifiedTime = DateTime.Now;
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<NewsDto>(news);
        }
    }
}
