namespace UserManagement.Core.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }  // e.g., "Admin", "User"
        public string Description { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
