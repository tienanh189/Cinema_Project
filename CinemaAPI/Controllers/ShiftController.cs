using CinemaAPI.Helpers;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Security.Claims;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : _ControllerBase
    {
        private readonly IShiftRespository _repo;
        private readonly IHttpContextAccessor _contextAccessor;

        public ShiftController(IShiftRespository repo, IHttpContextAccessor contextAccessor)
        {
            _repo = repo;
            _contextAccessor = contextAccessor;
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> Create(ShiftDto dto)
        {
            try
            {
                if (_contextAccessor.HttpContext != null)
                {
                    Guid adminID;
                    Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminID);
                    var shift = await _repo.Create(dto, adminID);
                    if (shift.ShiftId == Guid.Empty)
                    {
                        return Ok("Create Failed");
                    }
                    return Ok(shift);
                }
                return Ok("Create Failed");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("id")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> Update(Guid id, [FromBody] ShiftDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (_contextAccessor.HttpContext != null)
                {
                    Guid adminID;
                    Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminID);
                    var shift = await _repo.Update(id, dto, adminID);
                    if (shift.ShiftId == Guid.Empty)
                    {
                        return Ok("Update Failed");
                    }
                    return Ok(shift);
                }
                return Ok("Update Failed");
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
