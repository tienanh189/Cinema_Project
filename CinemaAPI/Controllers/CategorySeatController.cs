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
    public class CategorySeatController : _ControllerBase
    {
        private readonly ICategorySeatRespository _repo;

        public CategorySeatController(ICategorySeatRespository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
        {
            try
            {
                var categorySeats = await _repo.GetAll();
                return Ok(await GetPaginatedResponse(categorySeats, pagination));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var categorySeat = await _repo.GetById(id);
            return categorySeat == null ? NotFound() : Ok(categorySeat);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> Create(CategorySeatDto dto)
        {
            try
            {
                var categorySeat = await _repo.Create(dto);
                return Ok(categorySeat);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("id")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> Update(Guid id, [FromBody] CategorySeatDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var categorySeat = await _repo.Update(id, dto);
                return Ok(categorySeat);
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
