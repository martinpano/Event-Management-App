var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache")
    .WithDataVolume()
    .WithRedisCommander();

var apiService = builder.AddProject<Projects.EventManager_ApiService>("apiservice")
    .WithReference(cache)
    .WaitFor(cache);

builder.AddProject<Projects.EventManager_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
