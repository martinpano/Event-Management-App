using EventManager.Db;
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

        }
    }
}
