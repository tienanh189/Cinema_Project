namespace CinemaAPI.Models
{
    public class BaseModel
    {
        public Guid? CreatedByUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public Guid? ModifiedByUser { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public DateTime? DeletedTime { get; set; }
        public bool IsDeleted { get; set; } 

    }
}
