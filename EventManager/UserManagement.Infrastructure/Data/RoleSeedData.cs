using UserManagement.Core.Entities;

namespace UserManagement.Infrastructure.Data
{
    public static class RoleSeedData
    {
        public static List<Role> SeedRoles()
        {
            return
            [
                new() { Id = 1, Name = "Admin", Description = "Admin role with all the privileges." },
                new() { Id = 2, Name = "User", Description = "User role with regular privileges."}
            ];
        }
    }
}
