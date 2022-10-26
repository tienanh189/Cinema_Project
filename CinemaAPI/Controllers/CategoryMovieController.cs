using CinemaAPI.Helpers;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryMovieController : _ControllerBase
    {
        private readonly ICategoryMovieRespository _repo;

        public CategoryMovieController(ICategoryMovieRespository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
        {
            try
            {
                var categoryMovies = await _repo.GetAll();
                return Ok(await GetPaginatedResponse(categoryMovies, pagination));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var categoryMovie = await _repo.GetById(id);
            return categoryMovie == null ? NotFound() : Ok(categoryMovie);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryMovieDto dto)
        {
            try
            {
                var categoryMovie = await _repo.Create(dto);
                return Ok(categoryMovie);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryMovieDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var categoryMovie = await _repo.Update(id,dto);
                return Ok(categoryMovie);
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
