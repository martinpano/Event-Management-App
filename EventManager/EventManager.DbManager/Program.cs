using EventManager.Db;
using EventManager.DbManager;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<EventManagerDbContext>("eventdb", null,
    optionsBuilder => optionsBuilder.UseNpgsql(npgsqlBuilder =>
        npgsqlBuilder.MigrationsAssembly(typeof(Program).Assembly.GetName().Name)));

builder.Services.AddSingleton<EventDbInitializer>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<EventDbInitializer>());

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapPost("/reset-db", async (EventManagerDbContext dbContext, EventDbInitializer dbInitializer, CancellationToken cancellationToken) =>
    {
        // Delete and recreate the database. This is useful for development scenarios to reset the database to its initial state.
        await dbContext.Database.EnsureDeletedAsync(cancellationToken);
        await dbInitializer.InitializeDatabaseAsync(dbContext, cancellationToken);
    });
}


app.MapDefaultEndpoints();
await app.RunAsync();


