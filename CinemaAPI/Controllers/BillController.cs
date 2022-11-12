﻿using CinemaAPI.Helpers;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : _ControllerBase
    {
        private readonly IBillRespository _repo;
        private readonly ITicketRespository _repoTicket;
        private readonly ICategorySeatRespository _repoCategorySeat;
        private readonly ISeatRespository _repoSeat;
        private readonly ICinemaRespository _repoCinema;
        private readonly IShowTimeRespository _repoShowTime;
        private readonly IMovieRespository _repoMovie;
        private readonly IShiftRespository _repoShift;
        private readonly IRoomRespository _repoRoom;

        public BillController(IBillRespository repo, ITicketRespository repoTicket, ICategorySeatRespository repoCategorySeat, ISeatRespository repoSeat, ICinemaRespository repoCinema, IShowTimeRespository repoShowTime, IMovieRespository repoMovie, IShiftRespository repoShift, IRoomRespository repoRoom)
        {
            _repo = repo;
            _repoTicket = repoTicket;
            _repoCategorySeat = repoCategorySeat;
            _repoSeat = repoSeat;
            _repoCinema = repoCinema;
            _repoShowTime = repoShowTime;
            _repoMovie = repoMovie;
            _repoShift = repoShift;
            _repoRoom = repoRoom;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
        {
            try
            {
                var bill = await _repo.GetAll();
                return Ok(await GetPaginatedResponse(bill, pagination));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost("Bill")]
        public async Task<IActionResult> GetBillDetail([FromBody]BookingTicketDto dto)
        {
            try
            {
                var bill = await GetDetailBill(dto);
                return Ok(bill);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var bill = await _repo.GetById(id);
            return bill == null ? NotFound() : Ok(bill);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BillDto dto)
        {
            try
            {
                var bill = await _repo.Create(dto);
                return Ok(bill);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BillDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var bill = await _repo.Update(id, dto);
                return Ok(bill);
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

        #region
        private async Task<BillDetailDto> GetDetailBill(BookingTicketDto input)
        {
            var newBill = new BillDetailDto();
            var bill = new BillDto();      
            newBill.BillId = await _repo.CreatAndReturnId(bill);
            var categorySeat = await _repoCategorySeat.GetAll();
            var seats = await _repoSeat.GetAll();
            newBill.CinemaId = input.CinemaId;
            newBill.ShowTimeId = input.ShowTimeId;
            newBill.Date = input.Date;
            //Lấy tên rạp
            var cinemas = await _repoCinema.GetById(newBill.CinemaId);
            newBill.CinemaName = cinemas.CinemaName;
            //Lấy giờ chiếu
            var showTime = await _repoShowTime.GetById(newBill.ShowTimeId);
            var shift = await _repoShift.GetById(showTime.ShiftId);
            newBill.StartTime = shift.StartTime;
            //Lấy tên phim
            var movie = await _repoMovie.GetById(showTime.MovieId);
            newBill.MovieName = movie.MovieName;
            //Lây tên phòng
            var room = await _repoRoom.GetById(showTime.RoomId);
            newBill.RoomName = room.RoomName;

            //Thêm danh sách chi tiết thông tin về ghế đã chon bao gồm giá tiền
            foreach (var seat in input.ListSeat)
            {
                var seatDetailQuery = from s in seats
                                 join cs in categorySeat on s.CategorySeatId equals cs.CategorySeatId
                                 where s.SeatId == seat.SeatId
                                 select new SeatOnBillDto
                                 {
                                     SeatId = s.SeatId,
                                     SeatName = s.SeatName,
                                     Price = cs.Price,
                                 };
                newBill.ListSeat.Add(seatDetailQuery.SingleOrDefault());
            }
            //Lấy tổng tiền
            foreach (var seatDetail in newBill.ListSeat)
            {
                newBill.TotalAmount += seatDetail.Price;
            }

            //Cập nhật bill
            bill.BillId = newBill.BillId;
            bill.TotalAmount = newBill.TotalAmount;
            bill.IsPayed = false;
            await _repo.Update(newBill.BillId, bill);
            return newBill;
        }
        #endregion
    }
}
