using CinemaAPI.Models;
using CinemaAPI.Respositories.Interface;
using Microsoft.AspNetCore.Identity;

namespace CinemaAPI.Respositories
{
    public class RoleRespository : IRoleRespository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleMager;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public RoleRespository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, 
            RoleManager<IdentityRole> roleMager, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleMager = roleMager;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> Create(Role dto)
        {
            if (!await _roleMager.RoleExistsAsync(dto.Name))
            {
                await _roleMager.CreateAsync(dto);
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(string Id)
        {
            try
            {
                var role = await _roleMager.FindByIdAsync(Id);          
                await _roleMager.DeleteAsync(role);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public IEnumerable<IdentityRole> GetAll()
        {
            var roles = _roleMager.Roles.AsEnumerable();
            return roles;
        }

        public async Task<bool> Update(string Id, Role dto)
        {
            try
            {
                var role = await _roleMager.FindByIdAsync(Id);
                role.Name = dto.Name;
                role.NormalizedName = dto.NormalizedName;
                await _roleMager.UpdateAsync(role);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}
