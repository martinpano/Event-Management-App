using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.Core.Entities;

namespace UserManagement.Infrastructure.Data
{
    public static class SeedData
    {
        public static List<User> SeedUsers()
        {
            return
            [
                new()
                {
                    Id = 1,
                    Email = "admin@example.com",
                    PasswordHash = "AQAAAAIAAYagAAAAEJ/qN3Z0vmGQqLlAtbpvaJKYtxuezhFHOXHf8Ga1rffHAIvS0EOAdXQ/XryTIR2J0g==",
                    FirstName = "Admin",
                    LastName = "User",
                    City = "New York"
                },
                new()
                {
                    Id = 2,
                    Email = "testuser@example.com",
                    PasswordHash = "AQAAAAIAAYagAAAAEE5uOidl8ku2IQFkktpun2EwAF4zsRQYHmaklVYLXroPrmWgpWR/7D7es789a70ttA==",
                    FirstName = "Test",
                    LastName = "User",
                    City = "Los Angeles"
                }
            ];
        }

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
