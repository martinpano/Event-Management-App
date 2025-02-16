namespace UserManagement.Core.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }  // e.g., "Admin", "User"
    }
}
