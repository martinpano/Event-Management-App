var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache")
    .WithDataVolume()
    .WithRedisCommander();

var userManagementService = builder.AddProject<Projects.EventManager_UserManagementService>("eventmanager-usermanagementservice")
    .WithReference(cache)
    .WaitFor(cache);
builder.AddProject<Projects.EventManager_EventManagementService>("eventmanager-eventmanagementservice")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache);

builder.AddProject<Projects.EventManager_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(userManagementService)
    .WaitFor(userManagementService);


builder.Build().Run();
