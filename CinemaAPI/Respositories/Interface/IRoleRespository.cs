using CinemaAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace CinemaAPI.Respositories.Interface
{
    public interface IRoleRespository
    {
        public IEnumerable<IdentityRole> GetAll();
        public Task<bool> Create(Role dto);
        public Task<bool> Update(string Id,Role dto);
        public Task<bool> Delete(string Id);
    }
}
