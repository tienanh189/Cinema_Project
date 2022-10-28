using AutoMapper;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

    public class BillRespository : IBillRespository
    {
        private readonly CinemaDbContext _db;
        private readonly IMapper _mapper;

        public BillRespository(CinemaDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<BillDto> Create(BillDto dto)
        {
            var bill = _mapper.Map<Bill>(dto);
            bill.BillId = Guid.NewGuid();
            bill.CreatedTime = DateTime.Now;
            bill.ModifiedTime = null;
            bill.DeletedTime = null;
            bill.CreatedByUser = null;
            bill.ModifiedByUser = null;
            bill.IsDeleted = false;
            _db.Bill.Add(bill);
            await _db.SaveChangesAsync();
            return _mapper.Map<BillDto>(bill);
        }

        public async Task<bool> Delete(Guid id)
        {
            var bill = await _db.Bill.FindAsync(id);
            if (bill != null)
            {
                bill.IsDeleted = true;
                bill.DeletedTime = DateTime.Now;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IQueryable<BillDto>> GetAll()
        {
            var bill = _db.Bill.Where(x => x.IsDeleted == false).AsEnumerable();
            return _mapper.Map<List<BillDto>>(bill).AsQueryable();
        }

        public async Task<BillDto> GetById(Guid id)
        {
            var bill = _db.Bill.Where(x => x.IsDeleted == false && x.BillId == id).AsEnumerable();
            return _mapper.Map<BillDto>(bill.FirstOrDefault());
        }

        public async Task<BillDto> Update(Guid id, BillDto dto)
        {
            var bill = await _db.Bill.FindAsync(id);
            if (bill != null)
            {
                bill.TotalAmount = dto.TotalAmount;
                bill.DiscountId = dto.DiscountId;
                bill.ModifiedTime = DateTime.Now;
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<BillDto>(bill);
        }
    }

