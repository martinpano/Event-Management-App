using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using UserManagement.Core.Entities;

namespace UserManagement.Infrastructure.Data
{
    public class UserDbInitializer(IServiceProvider serviceProvider, ILogger<UserDbInitializer> logger)
    : BackgroundService
    {
        public const string ActivitySourceName = "Migrations";

        private readonly ActivitySource _activitySource = new(ActivitySourceName);

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<UserManagementDbContext>();

            using var activity = _activitySource.StartActivity("Initializing catalog database", ActivityKind.Client);

            await EnsureDatabaseAsync(dbContext, cancellationToken);

            await InitializeDatabaseAsync(dbContext, cancellationToken);
        }

        private static async Task EnsureDatabaseAsync(UserManagementDbContext dbContext, CancellationToken cancellationToken)
        {
            var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                // Create the database if it does not exist.
                // Do this first so there is then a database to start a transaction against.
                if (!await dbCreator.ExistsAsync(cancellationToken))
                {
                    await dbCreator.CreateAsync(cancellationToken);
                }
            });
        }

        public async Task InitializeDatabaseAsync(UserManagementDbContext dbContext, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();

            var strategy = dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(dbContext.Database.MigrateAsync, cancellationToken);

            await SeedAsync(dbContext, cancellationToken);

            logger.LogInformation("Database initialization completed after {ElapsedMilliseconds}ms", sw.ElapsedMilliseconds);
        }

        private async Task SeedAsync(UserManagementDbContext dbContext, CancellationToken cancellationToken)
        {
            logger.LogInformation("Seeding database");

            static List<User> GetPreconfiguredUsers()
            {
                return SeedData.SeedUsers();
            }

            static List<Role> GetPreconfiguredRoles()
            {
                return SeedData.SeedRoles();
            }


            if (!dbContext.Users.Any())
            {
                var users = GetPreconfiguredUsers();
                await dbContext.Users.AddRangeAsync(users, cancellationToken);

                logger.LogInformation("Seeding {UserCount} users", users.Count);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            if (!dbContext.Roles.Any())
            {
                var roles = GetPreconfiguredRoles();
                await dbContext.Roles.AddRangeAsync(roles, cancellationToken);

                logger.LogInformation("Seeding {EventCount} events", roles.Count);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
