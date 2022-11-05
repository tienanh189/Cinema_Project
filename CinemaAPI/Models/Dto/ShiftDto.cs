namespace CinemaAPI.Models.Dto
{
    public class ShiftDto : BaseModel
    {
        public Guid ShiftId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
