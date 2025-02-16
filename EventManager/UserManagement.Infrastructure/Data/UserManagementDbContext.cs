using Microsoft.EntityFrameworkCore;
using UserManagement.Core.Entities;
using UserManagement.Infrastructure.Data.EntityConfigurations;

namespace UserManagement.Infrastructure.Data
{
    public class UserManagementDbContext(DbContextOptions<UserManagementDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleEntityTypeConfiguration());

            modelBuilder.Entity<User>()
                .HasData(UserSeedData.SeedUsers());
            modelBuilder.Entity<Role>()
                .HasData(RoleSeedData.SeedRoles());
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { UserId = 1, RoleId = 1 },
                new UserRole { UserId = 2, RoleId = 2 }
            );

        }
    }
}
