using CinemaAPI.Helpers;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : _ControllerBase
    {
        private readonly IMovieRespository _repoMovie;
        private readonly ICategoryMovieRespository _categoryMovie;
        private readonly ICategoryMovie_MovieRespository _categoryMovie_Movie;

        public MovieController(IMovieRespository repoMovie, ICategoryMovieRespository categoryMovie, ICategoryMovie_MovieRespository categoryMovie_MovieRespository)
        {
            _repoMovie = repoMovie;
            _categoryMovie = categoryMovie;
            _categoryMovie_Movie = categoryMovie_MovieRespository;
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] Pagination pagination)
        {
            try
            {
                var movies = await _repoMovie.GetAll();
                return Ok(await GetPaginatedResponse(movies, pagination));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var movies = await GetAllMovies();
                return Ok(movies);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("admin")]
        public async Task<IActionResult> GetAllMoviesWithCategory()
        {
            try
            {
                var movies = await GetAllMovieDetailHelper();
                return Ok(movies);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var movie = await _repoMovie.GetById(id);
            return movie == null ? NotFound() : Ok(movie);
        }

        [HttpGet("detailMovieId")]
        public async Task<IActionResult> GetMovieDetail(Guid detailMovieId)
        {
            var movie = await GetMovieDetailHelper(detailMovieId);
            return movie == null ? NotFound() : Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieDto dto)
        {
            try
            {
                var movie = await _repoMovie.Create(dto);
                return Ok(movie);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(Guid id, [FromBody] MovieDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var movie = await _repoMovie.Update(id, dto);
                return Ok(movie);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _repoMovie.Delete(id);
                return result == true ? Ok(result) : NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        #region API Helers

        //Get a movie detail
        private async Task<MovieDetail> GetMovieDetailHelper(Guid Id)
        {
            var movies = await _repoMovie.GetAll();
            var categorymovie = await _categoryMovie.GetAll();
            var cateMovie_Movie = await _categoryMovie_Movie.GetAll();

            var movieDetails = from cmm in cateMovie_Movie
                               join m in movies on cmm.MovieId equals m.MovieId
                               join cm in categorymovie on cmm.CategoryMovieId equals cm.CategoryMovieId
                               where cmm.MovieId == Id
                               select new
                               {
                                   MovieId = Id,
                                   MovieName = m.MovieName,
                                   Duration = m.Duration,
                                   Actor = m.Actor,
                                   Director = m.Director,
                                   Description = m.MovieDescription,
                                   ReleaseDate = m.ReleaseDate,
                                   Image = m.Image,
                                   CategoryMovieName = cm.CategoryMovieName
                               };
            //6e1a8cd3-ecbe-40d7-950b-6757abbbfa10
            var movieDetail = new MovieDetail(Guid.Empty, "null", "null", 12, "null", "null", "null", DateTime.Now, false);
            foreach (var movie in movieDetails)
            {
                movieDetail.MovieId = movie.MovieId;
                movieDetail.MovieName = movie.MovieName;
                movieDetail.Duration = movie.Duration;
                movieDetail.ReleaseDate = movie.ReleaseDate;
                movieDetail.MovieDescription = movie.Description;
                movieDetail.Actor = movie.Actor;
                movieDetail.Image = movie.Image;
                movieDetail.Director = movie.Director;
                movieDetail.ListCategoryMovieName.Add(movie.CategoryMovieName);
            }
            return movieDetail;
        }

        // Get all movies with all categorymovies name of them
        private async Task<List<MovieDetail>> GetAllMovieDetailHelper()
        {
            var movies = await _repoMovie.GetAll();
            var categorymovie = await _categoryMovie.GetAll();
            var cateMovie_Movie = await _categoryMovie_Movie.GetAll();

            var movieDetails = from cmm in cateMovie_Movie
                               join m in movies on cmm.MovieId equals m.MovieId
                               join cm in categorymovie on cmm.CategoryMovieId equals cm.CategoryMovieId
                               select new
                               {
                                   MovieId = m.MovieId,
                                   MovieName = m.MovieName,
                                   Duration = m.Duration,
                                   Actor = m.Actor,
                                   Director = m.Director,
                                   Description = m.MovieDescription,
                                   ReleaseDate = m.ReleaseDate,
                                   Image = m.Image,
                                   CategoryMovieName = cm.CategoryMovieName
                               };
            //6e1a8cd3-ecbe-40d7-950b-6757abbbfa10
            var movieDetailList = new List<MovieDetail>();
            foreach (var movie in movieDetails)
            {
                var movieDetail = new MovieDetail(movie.MovieId, movie.MovieName, movie.Description,
                    movie.Duration, movie.Actor, movie.Director, movie.Image, movie.ReleaseDate, false);
                movieDetail.ListCategoryMovieName.Add(movie.CategoryMovieName);
                movieDetailList.Add(movieDetail);
                
            }
            return movieDetailList;
        }

        // Get all movies with type: IsShowing 
        private async Task<List<MovieDto>> GetAllMovies()
        {
            var movies = await _repoMovie.GetAll();
            foreach (var movie in movies)
            {
                int result = 0;
                DateTime today = DateTime.Now;
                DateTime realseDay = movie.ReleaseDate;
                result = (int)(today - realseDay).TotalDays;
                if (result<=14)
                {
                    movie.IsShowing = true;
                }
                else
                {
                    movie.IsShowing = false;
                }
            }
            return movies.ToList();
        } 

        #endregion
    }
}
