using CinemaAPI.Helpers;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowTime_SeatController : _ControllerBase
    {
        private readonly IShowTime_SeatRespository _repo;

        public ShowTime_SeatController(IShowTime_SeatRespository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
        {
            try
            {
                var showTime_Seats = await _repo.GetAll();
                return Ok(await GetPaginatedResponse(showTime_Seats, pagination));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var showTime_Seats = await _repo.GetById(id);
            return showTime_Seats == null ? NotFound() : Ok(showTime_Seats);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ShowTime_SeatDto dto)
        {
            try
            {
                var showTime_Seats = await _repo.Create(dto);
                return Ok(showTime_Seats);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ShowTime_SeatDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var showTime_Seats = await _repo.Update(id, dto);
                return Ok(showTime_Seats);
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
    }
}
