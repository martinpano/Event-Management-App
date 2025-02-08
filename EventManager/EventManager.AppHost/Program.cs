var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .WithLifetime(ContainerLifetime.Persistent);

if (builder.ExecutionContext.IsRunMode)
{
    // Data volumes don't work on ACA for Postgres so only add when running
    postgres.WithDataVolume();
}

var eventDb = postgres.AddDatabase("eventDb");


var cache = builder.AddRedis("cache")
    .WithDataVolume()
    .WithRedisCommander();

var eventDbManager = builder.AddProject<Projects.EventManager_DbManager>("eventmanager-eventdbmanager")
    .WithReference(eventDb)
    .WaitFor(eventDb);
    //.WithHttpHealthCheck("/health")
    //.WithHttpsCommand("/reset-db", "Reset Database", iconName: "DatabaseLightning");

var userManagementService = builder.AddProject<Projects.EventManager_UserManagementService>("eventmanager-usermanagementservice")
    .WithReference(cache)
    .WaitFor(cache);

builder.AddProject<Projects.EventManager_EventManagementService>("eventmanager-eventmanagementservice")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache);


var bookingService = builder.AddProject<Projects.EventManager_BookingService>("eventmanager-bookingservice")
    .WithReference(cache)
    .WaitFor(cache);


builder.AddProject<Projects.EventManager_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(userManagementService)
    .WaitFor(userManagementService);


builder.AddProject<Projects.EventManager_BookingService>("eventmanager-bookingservice");


builder.Build().Run();
