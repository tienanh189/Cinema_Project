using CinemaAPI.Helpers;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("getMyBill")]
        public async Task<IActionResult> GetMyBill()
        {
            try
            {
                var bills = await _repo.GetMyBill();
                var showTimes = await _repoShowTime.GetAll();
                var tickets = await _repoTicket.GetAll();
                var movies = await _repoMovie.GetAll();
                var shifts = await _repoShift.GetAll();
                var rooms = await _repoRoom.GetAll();
                var cinemas = await _repoCinema.GetAll();

                var result = from t in tickets
                             join b in bills on t.BillId equals b.BillId
                             join st in showTimes on t.ShowTimeId equals st.ShowTimeId
                             join m in movies on st.MovieId equals m.MovieId
                             join s in shifts on st.ShiftId equals s.ShiftId
                             join r in rooms on st.RoomId equals r.RoomId
                             join c in cinemas on r.CinemaId equals c.CinemaId
                             select new
                             {
                                 BillId = b.BillId,
                                 MovieName = m.MovieName,
                                 RoomName = r.RoomName,
                                 CinemaName = c.CinemaName,
                                 StartTime = s.StartTime,
                                 ShowDate = st.ShowDate,
                                 totalAmount = b.TotalAmount
                             };
                return Ok(result.Distinct());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost("getBill")]

        public async Task<IActionResult> GetBillDetail([FromBody]BillDetailDto dto)
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

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
        private async Task<BillDetailDto> GetDetailBill(BillDetailDto input)
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
