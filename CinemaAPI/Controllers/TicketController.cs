using CinemaAPI.Helpers;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : _ControllerBase
    {
        private readonly ITicketRespository _repo;
        private readonly IBillRespository _repoBill;
        private readonly ICategorySeatRespository _repoCategorySeat;
        private readonly ISeatRespository _repoSeat;
        private readonly ICinemaRespository _repoCinema;

        public TicketController(ITicketRespository repo, IBillRespository repoBill, ICategorySeatRespository repoCategorySeat, ISeatRespository repoSeat, ICinemaRespository repoCinema)
        {
            _repo = repo;
            _repoBill = repoBill;
            _repoCategorySeat = repoCategorySeat;
            _repoSeat = repoSeat;
            _repoCinema = repoCinema;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
        {
            try
            {
                var tickets = await _repo.GetAll();
                return Ok(await GetPaginatedResponse(tickets, pagination));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var ticket = await _repo.GetById(id);
            return ticket == null ? NotFound() : Ok(ticket);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BillDetailDto dto)
        {
            try
            {
                var ticket = await CreateTicket(dto);
                return Ok(ticket);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TicketDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var ticket = await _repo.Update(id, dto);
                return Ok(ticket);
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

        private async Task<BillDetailDto> CreateTicket(BillDetailDto input)
        {
            var categorySeat = await _repoCategorySeat.GetAll();
            foreach (var seatDetail in input.ListSeat)
            {
                var ticket = new TicketDto();
                ticket.BillId = input.BillId;
                ticket.ShowTimeId = input.ShowTimeId;
                ticket.Price = seatDetail.Price;
                ticket.SeatId = seatDetail.SeatId;
                await _repo.Create(ticket);
            }     
            return input;
        }


        #endregion
    }
}
