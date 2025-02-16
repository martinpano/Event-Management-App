using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.Core.Entities;

namespace UserManagement.Infrastructure.Data
{
    public static class SeedData
    {
        public static List<User> SeedUsers()
        {
            var hasher = new PasswordHasher<User>();
            return
            [
                new()
                {
                    Id = 1,
                    Email = "admin@example.com",
                    PasswordHash = hasher.HashPassword(null, "Admin@123"), // ❌ Will not work in migrations
                    FirstName = "Admin",
                    LastName = "User",
                    City = "New York"
                },
                new()
                {
                    Id = 2,
                    Email = "testuser@example.com",
                    PasswordHash = hasher.HashPassword(null, "User@123"), // ❌ Will not work in migrations
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
