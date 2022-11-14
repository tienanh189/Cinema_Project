using CinemaAPI.Models.Dto;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories.Interface
{
    public interface IBillRespository
    {
        public Task<IQueryable<BillDto>> GetAll();
        public Task<BillDto> GetById(Guid id);
        public Task<BillDto> Create(BillDto dto);
        public Task<Guid> CreatAndReturnId(BillDto dto);
        public Task<BillDto> Update(Guid id, BillDto dto);
        public Task<IQueryable<BillDto>> GetMyBill();
        public Task<bool> Delete(Guid id);
    }
}
