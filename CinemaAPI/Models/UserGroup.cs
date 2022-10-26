namespace CinemaAPI.Models
{
    public class UserGroup : BaseModel
    {
        public Guid UserGroupId { get; set; }
        public string? UserGroupName { get; set; }
        public ICollection<UserGroup_Permission> UserGroup_Permissions { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
