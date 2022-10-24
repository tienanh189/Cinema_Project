namespace CinemaAPI.Models
{
    public class User : BaseModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Sex { get; set; }
        public Guid UserGroupId { get; set; }
        public UserGroup UserGroups { get; set; }
    }
}
