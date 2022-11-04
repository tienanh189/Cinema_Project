namespace CinemaAPI.Models
{
    public class Bill : BaseModel
    {
        public Guid BillId { get; set; }
        public float TotalAmount { get; set; }
        public ICollection<Ticket> Tickets { get;}

    }
}
