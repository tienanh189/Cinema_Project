namespace CinemaAPI.Models
{
    public class Permission : BaseModel
    {
        public Guid PermissionId { get; set; }
        public string PermissionName { get; set; }
        public ICollection<UserGroup_Permission> UserGroup_Permissions { get; set; }
    }
}
