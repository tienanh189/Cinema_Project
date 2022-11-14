using CinemaAPI.Helpers;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("getall")]
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

        [HttpGet("getmovieshowtime")]
        public async Task<IActionResult> GetAllMoviesShowTime()
        {
            try
            {
                var listMovie = new List<MovieDto>();
                var movies = await _repoMovie.GetAll();           
                foreach (var movie in movies.ToList())
                {
                    DateTime today = DateTime.Now.Date;
                    DateTime endDate = movie.EndShowDate.Date;
                    int time = (int)(today - endDate).TotalDays;
                    if (time <= 0)
                    {
                        listMovie.Add(movie);
                    }
                }
                return Ok(listMovie);
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

        [HttpPost("create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

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

        [HttpPost("Save")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SaveMovie(CreateMovieDto dto)
        {
            try
            {
                var movie = await CreateOrUpdateMovie(dto);
                return Ok(movie);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("id")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        //Create new movie
        private async Task<CreateMovieDto> CreateOrUpdateMovie(CreateMovieDto input)
        {
            if (input.MovieId == Guid.Empty)
            {
                var movies = await _repoMovie.GetAll();
                movies = movies.Where(x => x.MovieName == input.MovieName && x.Director == input.Director);
                if (movies != null)
                {
                    var newMovie = new MovieDto();
                    newMovie.MovieName = input.MovieName;
                    newMovie.MovieDescription = input.MovieDescription;
                    newMovie.Duration = input.Duration;
                    newMovie.Actor = input.Actor;
                    newMovie.Director = input.Director;
                    newMovie.ReleaseDate = input.ReleaseDate;
                    newMovie.EndShowDate = input.EndShowDate;
                    newMovie.Image = input.Image;
                    newMovie.MovieId = await _repoMovie.CreateAndReturnId(newMovie);

                    foreach (var category in input.CategoryMovies)
                    {
                        var cateM_M = new CategoryMovie_MovieDto();
                        cateM_M.MovieId = newMovie.MovieId;
                        cateM_M.CategoryMovieId = category.CategoryMovieId;
                        await _categoryMovie_Movie.Create(cateM_M);
                    }
                }
            }
            else
            {
                var cateM_M = await _categoryMovie_Movie.GetAll();
                cateM_M = cateM_M.Where(x => x.MovieId == input.MovieId);
                var newMovie = new MovieDto();
                newMovie.MovieName = input.MovieName;
                newMovie.MovieDescription = input.MovieDescription;
                newMovie.Duration = input.Duration;
                newMovie.Actor = input.Actor;
                newMovie.Director = input.Director;
                newMovie.ReleaseDate = input.ReleaseDate;
                newMovie.EndShowDate = input.EndShowDate;
                newMovie.Image = input.Image;
                await _repoMovie.Update(input.MovieId, newMovie);

                foreach (var category in cateM_M)
                {
                    await _categoryMovie_Movie.Delete(category.Id);
                }

                foreach (var category in input.CategoryMovies)
                {
                    var newcateM_M = new CategoryMovie_MovieDto();
                    newcateM_M.MovieId = input.MovieId;
                    newcateM_M.CategoryMovieId = category.CategoryMovieId;
                    await _categoryMovie_Movie.Create(newcateM_M);
                }
            }
            return input;
        }

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
                                   CategoryMovieId = cm.CategoryMovieId,
                                   CategoryMovieName = cm.CategoryMovieName
                               };
            var movieDetail = new MovieDetail(Guid.Empty, "null", "null", 12, "null", "null", "null", DateTime.Now,DateTime.Now, false);
            foreach (var movie in movieDetails)
            {
                var cateM = new CategoryMovieDto();
                movieDetail.MovieId = movie.MovieId;
                movieDetail.MovieName = movie.MovieName;
                movieDetail.Duration = movie.Duration;
                movieDetail.ReleaseDate = movie.ReleaseDate;
                movieDetail.MovieDescription = movie.Description;
                movieDetail.Actor = movie.Actor;
                movieDetail.Image = movie.Image;
                movieDetail.Director = movie.Director;
                cateM.CategoryMovieId = movie.CategoryMovieId;  
                cateM.CategoryMovieName = movie.CategoryMovieName;
                movieDetail.CategoryMovies.Add(cateM);
            }         
            if (movieDetail.MovieId == Guid.Empty)
            {
                MovieDto movie = await _repoMovie.GetById(Id);
                movieDetail.MovieId = movie.MovieId;
                movieDetail.MovieName = movie.MovieName;
                movieDetail.Duration = movie.Duration;
                movieDetail.ReleaseDate = movie.ReleaseDate;
                movieDetail.MovieDescription = movie.MovieDescription;
                movieDetail.Actor = movie.Actor;
                movieDetail.Image = movie.Image;
                movieDetail.Director = movie.Director;
            }
            DateTime today = DateTime.Now.Date;
            DateTime realseDay = movieDetail.ReleaseDate.Date;
            DateTime endDate = movieDetail.EndShowDate.Date;
            int timeStart = (int)(today - realseDay).TotalDays;
            int timeEnd = (int)(endDate - today).TotalDays;
            if (timeStart >= 0 && timeEnd >= 0)
            {
                movieDetail.IsShowing = true;
            }
            else
            {
                movieDetail.IsShowing = false;
            }
            return movieDetail;
        }

        // Get all movies with all categorymovies name of them
        private async Task<List<MovieDetail>> GetAllMovieDetailHelper()
        {
            var movies = await _repoMovie.GetAll();
            var movieDetailList = new List<MovieDetail>();
            foreach (var movie in movies.ToList())
            {
                var movieDetail = new MovieDetail(Guid.Empty, "null", "null", 12, "null", "null", "null", DateTime.Now, DateTime.Now, false);
                movieDetail = await GetMovieDetailHelper(movie.MovieId);
                if (movieDetail.MovieId == Guid.Empty)
                {
                    movieDetail.MovieId = movie.MovieId;
                    movieDetail.MovieName = movie.MovieName;
                    movieDetail.Duration = movie.Duration;
                    movieDetail.ReleaseDate = movie.ReleaseDate;
                    movieDetail.MovieDescription = movie.MovieDescription;
                    movieDetail.Actor = movie.Actor;
                    movieDetail.Image = movie.Image;
                    movieDetail.Director = movie.Director;
                }
                DateTime today = DateTime.Now.Date;
                DateTime realseDay = movie.ReleaseDate.Date;
                DateTime endDate = movie.EndShowDate.Date;
                int timeStart = (int)(today - realseDay).TotalDays;
                int timeEnd = (int)(endDate - today).TotalDays;
                if (timeStart >= 0 && timeEnd >= 0)
                {
                    movieDetail.IsShowing = true;
                }
                else
                {
                    movieDetail.IsShowing = false;
                }
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
                DateTime today = DateTime.Now.Date;
                DateTime realseDay = movie.ReleaseDate.Date;
                DateTime endDate = movie.EndShowDate.Date;
                int timeStart = (int)(today - realseDay).TotalDays;
                int timeEnd = (int)(endDate - today).TotalDays;
                if ( timeStart >=0 &&  timeEnd>= 0)
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
