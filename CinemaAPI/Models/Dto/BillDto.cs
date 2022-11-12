using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Models.Dto
{
    public class BillDto : BaseModel
    {
        public Guid BillId { get; set; }
        public bool IsPayed { get; set; }
        public float TotalAmount { get; set; }
    }

    public class BillDetailDto : BillDto
    {
        public BillDetailDto()
        {
            ListSeat = new List<SeatOnBillDto>();
        }

        public Guid CinemaId { get; set; }
        public DateTime Date { get; set; }
        public Guid ShowTimeId { get; set; }
        public List<SeatOnBillDto>? ListSeat { get; set; }
        public string? MovieName { get; set; }
        public string? StartTime { get; set; }
        public string? RoomName { get; set; }
        public string? CinemaName { get; set; }

    }
}
