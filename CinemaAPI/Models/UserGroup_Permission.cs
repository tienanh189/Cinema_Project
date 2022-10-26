namespace CinemaAPI.Models
{
    public class UserGroup_Permission : BaseModel
    {
        public Guid Id { get; set; }
        public Guid PermissionId { get; set; }
        public Permission Permissions { get; set; }
        public Guid UserGroupId { get; set; }
        public UserGroup UserGroups { get; set; }
    }
}
