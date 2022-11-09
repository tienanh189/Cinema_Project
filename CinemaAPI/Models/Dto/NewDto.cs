using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
namespace CinemaAPI.Models.Dto
{
    public class NewDto : BaseModel
    {
        public Guid NewId { get; set; }
        public string? NewTittle { get; set; }
        public string? Description { get; set; }
        public string Image { get; set; }
    }
}
