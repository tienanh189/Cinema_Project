using CinemaAPI.Helpers;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowTimeController : _ControllerBase
    {
        private readonly IShowTimeRespository _repo;
        private readonly IShiftRespository _repoShift;
        private readonly IRoomRespository _repoRoom;
        private readonly ICinemaRespository _repoCinema;
        private readonly IMovieRespository _repoMovie;

        public ShowTimeController(IShowTimeRespository repo, IShiftRespository repoShift, IRoomRespository repoRoom, ICinemaRespository repoCinema, IMovieRespository repoMovie)
        {
            _repo = repo;
            _repoShift = repoShift;
            _repoRoom = repoRoom;
            _repoCinema = repoCinema;
            _repoMovie = repoMovie;
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] Pagination pagination)
        {
            try
            {
                var showTimes = await _repo.GetAll();
                return Ok(await GetPaginatedResponse(showTimes, pagination));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("bookingshowtime")]
        public async Task<IActionResult> GetAllShowTime(Guid CinemaId)
        {
            try
            {
                var showTimes = await GetShowTimeWithCinemaId(CinemaId);
                return Ok(showTimes);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var showTimes = await _repo.GetAll();
                return Ok(showTimes);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var showTime = await _repo.GetById(id);
            return showTime == null ? NotFound() : Ok(showTime);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ShowTimeDto dto)
        {
            try
            {
                var showTime = await _repo.Create(dto);
                return Ok(showTime);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ShowTimeDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var showTime = await _repo.Update(id, dto);
                return Ok(showTime);
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
                var result = await _repo.Delete(id);
                return result == true ? Ok(result) : NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #region  API Helper
        private async Task<List<ShowTimeDto>> GetShowTimeWithCinemaId(Guid CinemaId)
        {
            var cinemas = await _repoCinema.GetAll();
            var shifts = await _repoShift.GetAll();
            var rooms = await _repoRoom.GetAll();
            var showTimes = await _repo.GetAll();
            var movies = await _repoMovie.GetAll();

            var showTimeQuery = from st in showTimes
                                join s in shifts on st.ShiftId equals s.ShiftId
                                join r in rooms on st.RoomId equals r.RoomId
                                join m in movies on st.MovieId equals m.MovieId
                                where r.CinemaId == CinemaId
                                select new ShowTimeDto
                                {
                                    ShowTimeId = st.ShowTimeId,
                                    ShowDate = st.ShowDate,
                                    ShiftId = s.ShiftId,
                                    StartTime = s.StartTime,
                                    EndTime = s.EndTime,
                                    MovieId = st.MovieId,
                                    MovieName = m.MovieName,
                                    RoomId = r.RoomId,
                                    Duration = m.Duration,
                                    RoomName = r.RoomName
                                };

            return showTimeQuery.ToList();
        }
        #endregion

    }
}
