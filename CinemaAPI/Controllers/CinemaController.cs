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
    public class CinemaController : _ControllerBase
    {
        private readonly ICinemaRespository _repo;

        public CinemaController(ICinemaRespository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
        {
            try
            {
                var cinemas = await _repo.GetAll();
                return Ok(await GetPaginatedResponse(cinemas, pagination));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var cinema = await _repo.GetById(id);
            return cinema == null ? NotFound() : Ok(cinema);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> Create(CinemaDto dto)
        {
            try
            {
                var cinema = await _repo.Create(dto);
                return Ok(cinema);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("id")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> Update(Guid id, [FromBody] CinemaDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var cinema = await _repo.Update(id, dto);
                return Ok(cinema);
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
