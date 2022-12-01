using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRespository _repo;

        public AccountController(IAccountRespository repo)
        {
            _repo = repo;

        }

        [HttpGet("getUserInfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserInfo()
        {
            var result = _repo.GetUser();
            return Ok(result);
        }

        [HttpGet("getAllUser")]
        public IActionResult GetAllAccount()
        {
            try
            {
                var users = _repo.GetAll();
                return Ok(users);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpDto dto)
        {
            try
            {
                var result = await _repo.SignUpAsync(dto);
                if (result.Succeeded)
                {
                    return Ok(true);
                }
                return Ok(false);
            }
            catch (Exception e)
            {
                return Ok("failed");
            }
            
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInDto dto)
        {
            try
            {
                var result = await _repo.SignInAsync(dto);
                if (string.IsNullOrEmpty(result))
                {
                    return Ok("failed");
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok("failed");
            }          
        }

    }
}
