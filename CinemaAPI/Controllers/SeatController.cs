using CinemaAPI.Helpers;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : _ControllerBase
    {
        private readonly ISeatRespository _repoSeat;
        private readonly ITicketRespository _repoTicKet;
        private readonly IShowTimeRespository _repoShowTime;

        public SeatController(ISeatRespository repoSeat, ITicketRespository repoSeatTicKet, IShowTimeRespository showTimeRespository)
        {
            _repoSeat = repoSeat;
            _repoTicKet = repoSeatTicKet;
            _repoShowTime= showTimeRespository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
        {
            try
            {
                var seat = await _repoSeat.GetAll();
                return Ok(await GetPaginatedResponse(seat, pagination));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("detail")]
        public async Task<IActionResult> GetAllSeatInShowTime(Guid showTimeId)
        {
            try
            {
                var seat = await GetDetailAllSeats(showTimeId);
                return Ok(seat.ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var seat = await _repoSeat.GetById(id);
            return seat == null ? NotFound() : Ok(seat);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SeatDto dto)
        {
            try
            {
                var seat = await _repoSeat.Create(dto);
                return Ok(seat);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SeatDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var seat = await _repoSeat.Update(id, dto);
                return Ok(seat);
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
                var result = await _repoSeat.Delete(id);
                return result == true ? Ok(result) : NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #region API Helpers
        //54a9c024-a90f-41b1-9540-41d4bda61e7c
        private async Task<List<DetailSeat>> GetDetailAllSeats(Guid showTimeId)
        {
            var seatList = new List<DetailSeat>();
            var seats = await _repoSeat.GetAll();
            var tickets = await _repoTicKet.GetAll();
            var showTimes = await _repoShowTime.GetById(showTimeId);
            seats = seats.Where(x => x.RoomId == showTimes.RoomId).AsQueryable();

            var seatQuery = from t in tickets
                            join s in seats on t.SeatId equals s.SeatId
                            where t.ShowTimeId == showTimeId
                            select new
                            {
                                Id = s.SeatId,
                            };
            if(seatQuery != null)
            {
                foreach (var seat in seats)
                {
                    var detailSeat = new DetailSeat();
                    detailSeat.SeatId = seat.SeatId;
                    detailSeat.RoomId = seat.RoomId;
                    detailSeat.IsSelected = false;
                    detailSeat.SeatName = seat.SeatName;
                    foreach (var seatQ in seatQuery)
                    {
                        if (seat.SeatId == seatQ.Id)
                        {
                            detailSeat.IsSelected = true;
                        }
                    }
                    seatList.Add(detailSeat);
                }
            }
            else
            {
                foreach (var seat in seats)
                {
                    var detailSeat = new DetailSeat();
                    detailSeat.SeatId = seat.SeatId;
                    detailSeat.RoomId = seat.RoomId;
                    detailSeat.IsSelected = false;
                    detailSeat.SeatName = seat.SeatName;               
                    seatList.Add(detailSeat);
                }
            }
            
            return seatList;
        }
        #endregion
    }
}
