using CinemaAPI.Helpers;
using CinemaAPI.Models;
using CinemaAPI.Models.Dto;
using CinemaAPI.Respositories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        private readonly IHttpContextAccessor _contextAccessor;

        public TicketController(ITicketRespository repo, IBillRespository repoBill, ICategorySeatRespository repoCategorySeat, ISeatRespository repoSeat, ICinemaRespository repoCinema, IHttpContextAccessor contextAccessor)
        {
            _repo = repo;
            _repoBill = repoBill;
            _repoCategorySeat = repoCategorySeat;
            _repoSeat = repoSeat;
            _repoCinema = repoCinema;
            _contextAccessor = contextAccessor;
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

        [HttpGet("GetMyTicket")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetMyTicket()
        {
            try
            {
                if (_contextAccessor.HttpContext != null)
                {
                    Guid result;
                    var success = Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"),out result);
                    var tickets = await _repo.GetMyTicket(result);
                    return Ok(tickets.ToList());
                }
                return Ok("Create Failed");
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update(Guid id, [FromBody] TicketDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (_contextAccessor.HttpContext != null)
                {
                    Guid adminID;
                    Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminID);
                    var ticket = await _repo.Update(id, dto, adminID);
                    return Ok(ticket);
                }
                return Ok("Update Failed");
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

        private async Task<BillDetailDto> CreateTicket(BillDetailDto input)
        {
            var categorySeat = await _repoCategorySeat.GetAll();
            foreach (var seatDetail in input.ListSeat)
            {
                if (_contextAccessor.HttpContext != null)
                {
                    Guid adminID;
                    Guid.TryParse(_contextAccessor.HttpContext.User.FindFirstValue("UserId"), out adminID);
                    var ticket = new TicketDto();
                    ticket.BillId = input.BillId;
                    ticket.ShowTimeId = input.ShowTimeId;
                    ticket.Price = seatDetail.Price;
                    ticket.SeatId = seatDetail.SeatId;
                    await _repo.Create(ticket, adminID);
                }
            }     
            return input;
        }


        #endregion
    }
}
