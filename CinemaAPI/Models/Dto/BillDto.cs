using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Models.Dto
{
    public class BillDto : BaseModel
    {
        public Guid BillId { get; set; }
        public float TotalAmount { get; set; }
       
    }
}
