using CinemaAPI.Helpers;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Text;

namespace CinemaAPI.Respositories
{
    public class AccountRespository : IAccountRespository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleMager;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountRespository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, 
            RoleManager<IdentityRole> roleMager, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleMager = roleMager;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            var users = _userManager.Users.Where(x => x.IsDeleted == false).AsEnumerable();
            return users;
        }

        public UserDto GetUser()
        {
            var result = new UserDto();
            if (_contextAccessor.HttpContext != null)
            {
                result.FullNamme = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                result.Email = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                result.PhoneNumber = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.MobilePhone);
                result.Id = _contextAccessor.HttpContext.User.FindFirstValue("UserId");            
                result.Role = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            }
            return result;
        }

        public async Task<string> SignInAsync(SignInDto dto)
        {
            var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);
            if (!result.Succeeded)
            {
                return string.Empty;
            }
            var user = await _userManager.FindByNameAsync(dto.Email);
            var roles = await _userManager.GetRolesAsync(user);
            string listRole = "";
            foreach (var role in roles)
            {
                listRole += role.ToLowerInvariant();
            }
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, dto.Email),
                new Claim("UserId", user.Id),
                new Claim(ClaimTypes.Name, user.FullNamme),
                new Claim(ClaimTypes.Role, listRole),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(30),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> SignUpAsync(SignUpDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,              
                FullNamme = dto.FullNamme,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded)
            {
                if (dto.RoleName==null)
                {
                    return await _userManager.AddToRoleAsync(user, "customer");
                }
                return await _userManager.AddToRolesAsync(user, dto.RoleName);
            }
            return result;        
        }
    }
}
