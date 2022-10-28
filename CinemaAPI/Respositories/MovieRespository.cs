using AutoMapper;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories
{
    public class MovieRespository : IMovieRespository
    {
        private readonly CinemaDbContext _db;
        private readonly IMapper _mapper;
        public MovieRespository(CinemaDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<MovieDto> Create(MovieDto dto)
        {
            var movie = _mapper.Map<Movie>(dto);
            movie.MovieId = Guid.NewGuid();
            movie.CreatedTime = DateTime.Now;
            movie.ModifiedTime = null;
            movie.DeletedTime = null;
            movie.CreatedByUser = null;
            movie.ModifiedByUser = null;
            movie.IsDeleted = false;
            _db.Movie.Add(movie);
            await _db.SaveChangesAsync();
            return _mapper.Map<MovieDto>(movie);
        }

        public async Task<bool> Delete(Guid id)
        {
            var movie = await _db.Movie.FindAsync(id);
            if (movie != null)
            {
                movie.IsDeleted = true;
                movie.DeletedTime = DateTime.Now;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IQueryable<MovieDto>> GetAll()
        {
            var movies = _db.Movie.Where(x => x.IsDeleted == false).AsEnumerable();
            return _mapper.Map<List<MovieDto>>(movies).AsQueryable();
        }

        public async Task<MovieDto> GetById(Guid id)
        {
            var movie = _db.Movie.Where(x => x.IsDeleted == false && x.MovieId == id).AsEnumerable();
            return _mapper.Map<MovieDto>(movie.FirstOrDefault());
        }

        public async Task<MovieDto> Update(Guid id, MovieDto dto)
        {
            var movie = await _db.Movie.FindAsync(id);
            if (movie != null)
            {
                movie.MovieName = dto.MovieName;
                movie.MovieDescription = dto.MovieDescription;
                movie.Duration = dto.Duration;
                movie.Actor = dto.Actor;
                movie.Director = dto.Director;
                movie.ReleaseDate = dto.ReleaseDate;
                movie.ModifiedTime = DateTime.Now;
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<MovieDto>(movie);
        }
    }
}
