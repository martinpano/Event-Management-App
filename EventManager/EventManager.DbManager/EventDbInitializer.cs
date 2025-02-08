using EventManager.Db;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace EventManager.DbManager
{
    internal class EventDbInitializer(IServiceProvider serviceProvider, ILogger<EventDbInitializer> logger)
    : BackgroundService
    {
        public const string ActivitySourceName = "Migrations";

        private readonly ActivitySource _activitySource = new(ActivitySourceName);

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<EventManagerDbContext>();

            using var activity = _activitySource.StartActivity("Initializing catalog database", ActivityKind.Client);
            await InitializeDatabaseAsync(dbContext, cancellationToken);
        }

        public async Task InitializeDatabaseAsync(EventManagerDbContext dbContext, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();

            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(dbContext.Database.MigrateAsync, cancellationToken);

            await SeedAsync(dbContext, cancellationToken);

            logger.LogInformation("Database initialization completed after {ElapsedMilliseconds}ms", sw.ElapsedMilliseconds);
        }

        private async Task SeedAsync(EventManagerDbContext dbContext, CancellationToken cancellationToken)
        {
            logger.LogInformation("Seeding database");

            static List<User> GetPreconfiguredUsers()
            {
                var hasher = new PasswordHasher<User>();

                return new List<User>
                {
                    new() { Email = "admin@example.com", PasswordHash = hasher.HashPassword(null, "Admin@123"), Role = "Admin" },
                    new() { Email = "user1@example.com", PasswordHash = hasher.HashPassword(null, "User@123"), Role = "User" },
                    new() { Email = "user2@example.com", PasswordHash = hasher.HashPassword(null, "User@123"), Role = "User" },
                    new() { Email = "organizer@example.com", PasswordHash = hasher.HashPassword(null, "Organizer@123"), Role = "Organizer" },
                    new() { Email = "moderator@example.com", PasswordHash = hasher.HashPassword(null, "Moderator@123"), Role = "Moderator" }
                };
            }

            static List<Event> GetPreconfiguredEvents()
            {
                return new List<Event>
                {
                    new() { Name = "Tech Conference", Description = "Latest trends in technology.", Date = DateTime.UtcNow.AddDays(10), Location = "New York", Capacity = 150 },
                    new() { Name = "AI Summit", Description = "AI and ML advancements.", Date = DateTime.UtcNow.AddDays(20), Location = "San Francisco", Capacity = 200 },
                    new() { Name = "Cloud Expo", Description = "Cloud computing trends.", Date = DateTime.UtcNow.AddDays(30), Location = "Seattle", Capacity = 250 },
                    new() { Name = "DevOps Days", Description = "DevOps best practices.", Date = DateTime.UtcNow.AddDays(40), Location = "Chicago", Capacity = 180 },
                    new() { Name = "Cybersecurity Forum", Description = "Latest in cybersecurity.", Date = DateTime.UtcNow.AddDays(50), Location = "Boston", Capacity = 220 }
                };
            }

            static List<Booking> GetPreconfiguredBookings(DbSet<User> users, DbSet<Event> events)
            {
                var user1 = users.First(u => u.Email == "user1@example.com");
                var user2 = users.First(u => u.Email == "user2@example.com");

                var event1 = events.First(e => e.Name == "Tech Conference");
                var event2 = events.First(e => e.Name == "AI Summit");

                return new List<Booking>
                {
                    new() { UserId = user1.Id, EventId = event1.Id, NumberOfTickets = 2, BookingDate = DateTime.UtcNow },
                    new() { UserId = user2.Id, EventId = event2.Id, NumberOfTickets = 1, BookingDate = DateTime.UtcNow },
                    new() { UserId = user1.Id, EventId = event2.Id, NumberOfTickets = 3, BookingDate = DateTime.UtcNow.AddDays(-1) },
                    new() { UserId = user2.Id, EventId = event1.Id, NumberOfTickets = 1, BookingDate = DateTime.UtcNow.AddDays(-2) },
                    new() { UserId = user1.Id, EventId = event1.Id, NumberOfTickets = 4, BookingDate = DateTime.UtcNow.AddDays(-3) }
                };
            }

            if (!dbContext.Users.Any())
            {
                var users = GetPreconfiguredUsers();
                await dbContext.Users.AddRangeAsync(users, cancellationToken);

                logger.LogInformation("Seeding {UserCount} users", users.Count);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            if (!dbContext.Events.Any())
            {
                var events = GetPreconfiguredEvents();
                await dbContext.Events.AddRangeAsync(events, cancellationToken);

                logger.LogInformation("Seeding {EventCount} events", events.Count);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            if (!dbContext.Bookings.Any())
            {
                var bookings = GetPreconfiguredBookings(dbContext.Users, dbContext.Events);
                await dbContext.Bookings.AddRangeAsync(bookings, cancellationToken);

                logger.LogInformation("Seeding {BookingCount} bookings", bookings.Count);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
