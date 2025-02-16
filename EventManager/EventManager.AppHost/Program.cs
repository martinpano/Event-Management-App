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
var userDb = postgres.AddDatabase("userdb");

var cache = builder.AddRedis("cache")
    .WithDataVolume()
    .WithRedisCommander();

//var eventDbManager = builder.AddProject<Projects.EventManager_DbManager>("eventmanager-eventdbmanager")
//    .WithReference(eventDb)
//    .WaitFor(eventDb);
    //.WithHttpHealthCheck("/health")
    //.WithHttpsCommand("/reset-db", "Reset Database", iconName: "DatabaseLightning");

var userManagementService = builder.AddProject<Projects.UserManagement_Api>("user-api")
    .WithReference(userDb)
    .WaitFor(userDb)
    .WithReference(cache)
    .WaitFor(cache);

var eventManagementService = builder.AddProject<Projects.EventManager_Api>("event-api")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache);


var bookingManagementService = builder.AddProject<Projects.BookingManager_Api>("bookingmanager-api");


var cartService = builder.AddProject<Projects.CartManager_Service>("cartmanager-service")
    .WithReference(cache)
    .WaitFor(cache);


builder.AddProject<Projects.EventManager_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(userManagementService)
    .WaitFor(userManagementService)
    .WithReference(eventManagementService)
    .WaitFor(eventManagementService)
    .WithReference(bookingManagementService)
    .WaitFor(bookingManagementService);



builder.Build().Run();
