using CinemaAPI.Helpers;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;


namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : _ControllerBase
    {
        private readonly IShiftRespository _repo;

        public ShiftController(IShiftRespository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
        {
            try
            {
                var shifts = await _repo.GetAll();
                return Ok(await GetPaginatedResponse(shifts, pagination));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var shift = await _repo.GetById(id);
            return shift == null ? NotFound() : Ok(shift);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ShiftDto dto)
        {
            try
            {
                var shift = await _repo.Create(dto);
                if(shift.ShiftId == Guid.Empty)
                {
                    return Ok("Create Failed");
                }
                return Ok(shift);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ShiftDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var shift = await _repo.Update(id, dto);
                if (shift.ShiftId == Guid.Empty)
                {
                    return Ok("Update Failed");
                }
                return Ok(shift);
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
