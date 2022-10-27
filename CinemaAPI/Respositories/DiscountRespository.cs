using AutoMapper;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Respositories
{
    public class DiscountRespository : IDiscountRespository 
    {
        private readonly CinemaDbContext _db;
        private readonly IMapper _mapper;

        public DiscountRespository(CinemaDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<DiscountDto> Create(DiscountDto dto)
        {
            var discount = _mapper.Map<Discount>(dto);
            discount.DiscountId = Guid.NewGuid();
            discount.CreatedTime = DateTime.Now;
            discount.ModifiedTime = null;
            discount.DeletedTime = null;
            discount.CreatedByUser = null;
            discount.ModifiedByUser = null;
            discount.IsDeleted = false;
            _db.Discount.Add(discount);
            await _db.SaveChangesAsync();
            return _mapper.Map<DiscountDto>(discount);
        }

        public async Task<bool> Delete(Guid id)
        {
            var discount = await _db.Discount.FindAsync(id);
            if (discount != null)
            {
                discount.IsDeleted = true;
                discount.DeletedTime = DateTime.Now;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IQueryable<DiscountDto>> GetAll()
        {
            var discount = _db.Discount.Where(x => x.IsDeleted == false).AsEnumerable();
            return _mapper.Map<List<DiscountDto>>(discount).AsQueryable();
        }

        public async Task<DiscountDto> GetById(Guid id)
        {
            var discount = _db.Discount.Where(x => x.IsDeleted == false && x.DiscountId == id).AsEnumerable();
            return _mapper.Map<DiscountDto>(discount.FirstOrDefault());
        }

        public async Task<DiscountDto> Update(Guid id, DiscountDto dto)
        {
            var discount = await _db.Discount.FindAsync(id);
            if (discount != null)
            {
                discount.DiscountName = dto.DiscountName;
                discount.DiscountDetail = dto.DiscountDetail;
                discount.ModifiedTime = DateTime.Now;
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<DiscountDto>(discount);
        }
    }
}
