using CinemaAPI.Helpers;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : _ControllerBase
    {
        private readonly IRoomRespository _repo;

        public RoomController(IRoomRespository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
        {
            try
            {
                var rooms = await _repo.GetAll();
                return Ok(await GetPaginatedResponse(rooms, pagination));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var room = await _repo.GetById(id);
            return room == null ? NotFound() : Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoomDto dto)
        {
            try
            {
                var room = await _repo.Create(dto);
                return Ok(room);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RoomDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var room = await _repo.Update(id, dto);
                return Ok(room);
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
