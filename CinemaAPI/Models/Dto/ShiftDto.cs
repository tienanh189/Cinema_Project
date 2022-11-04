namespace CinemaAPI.Models.Dto
{
    public class ShiftDto : BaseModel
    {
        public Guid ShiftId { get; set; }
        public DateTime StartShift { get; set; }
        public DateTime EndShift { get; set; }
    }
}
