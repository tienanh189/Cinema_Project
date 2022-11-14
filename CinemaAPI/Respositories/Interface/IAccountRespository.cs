using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using Microsoft.AspNetCore.Identity;

namespace CinemaAPI.Respositories.Interface
{
    public interface IAccountRespository
    {
        public Task<IdentityResult> SignUpAsync (SignUpDto dto);
        public Task<string> SignInAsync(SignInDto dto);
        public IEnumerable<ApplicationUser> GetAll();
        public UserDto GetUser();

    }
}
