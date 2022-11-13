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
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountController(IAccountRespository repo, IHttpContextAccessor contextAccessor)
        {
            _repo = repo;
            _contextAccessor = contextAccessor;
        }

        [HttpGet("getUserInfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserInfo()
        {
            var result = new ApplicationUser();
            if (_contextAccessor.HttpContext != null)
            {
                result.FullNamme = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                result.Email = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            }
            return Ok(result);
        }


        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpDto dto)
        {
            try
            {
                var result = await _repo.SignUpAsync(dto);
                if (result.Succeeded)
                {
                    return Ok(result.Succeeded);
                }
                return Ok("failed");
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
