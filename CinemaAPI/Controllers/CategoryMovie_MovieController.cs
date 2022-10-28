using CinemaAPI.Helpers;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;


namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryMovie_MovieController : _ControllerBase
    {
        private readonly ICategoryMovie_MovieRespository _repo;

        public CategoryMovie_MovieController(ICategoryMovie_MovieRespository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
        {
            try
            {
                var categoryM_M = await _repo.GetAll();
                return Ok(await GetPaginatedResponse(categoryM_M, pagination));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var categoryM_M = await _repo.GetById(id);
            return categoryM_M == null ? NotFound() : Ok(categoryM_M);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryMovie_MovieDto dto)
        {
            try
            {
                var categoryM_M = await _repo.Create(dto);
                return Ok(categoryM_M);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryMovie_MovieDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var categoryM_M = await _repo.Update(id, dto);
                return Ok(categoryM_M);
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
