var builder = DistributedApplication.CreateBuilder(args);

// Add PostgreSQL container for each service with explicit health check overrides
var authDb = builder.AddPostgres("auth-db")
    .WithEnvironment("POSTGRES_DB", "authdb")
    .WithEnvironment("POSTGRES_USER", "postgres")
    .WithEnvironment("POSTGRES_PASSWORD", "postgres")
    // Add overrides for health checks
    .WithEnvironment("PGCONNECT_TIMEOUT", "5")
    .WithEnvironment("POSTGRES_HOST_AUTH_METHOD", "trust");

var dancerDb = builder.AddPostgres("dancer-db")
    .WithEnvironment("POSTGRES_DB", "dancerdb")
    .WithEnvironment("POSTGRES_USER", "postgres")
    .WithEnvironment("POSTGRES_PASSWORD", "postgres")
    // Add overrides for health checks
    .WithEnvironment("PGCONNECT_TIMEOUT", "5")
    .WithEnvironment("POSTGRES_HOST_AUTH_METHOD", "trust");

var courseDb = builder.AddPostgres("course-db")
    .WithEnvironment("POSTGRES_DB", "coursedb")
    .WithEnvironment("POSTGRES_USER", "postgres")
    .WithEnvironment("POSTGRES_PASSWORD", "postgres")
    // Add overrides for health checks
    .WithEnvironment("PGCONNECT_TIMEOUT", "5")
    .WithEnvironment("POSTGRES_HOST_AUTH_METHOD", "trust");

// Add RabbitMQ for messaging between services
var messageBus = builder.AddRabbitMQ("rabbitmq");

// Add the Authentication service
var authService = builder.AddProject<Projects.DanceChoreographyManager_Services_Authentication>("auth-service")
    .WithReference(authDb)
    .WithReference(messageBus);

// Add the Dancer service
var dancerService = builder.AddProject<Projects.DanceChoreographyManager_Services_Dancer>("dancer-service")
    .WithReference(dancerDb)
    .WithReference(messageBus);

// Add the Course service
var courseService = builder.AddProject<Projects.DanceChoreographyManager_Services_Course>("course-service")
    .WithReference(courseDb)
    .WithReference(messageBus);

// TODO: Add Choreography service
// TODO: Add Formation service 
// TODO: Add AI Assistant service

// TODO: Add API Gateway when ready
// var apiGateway = builder.AddProject<Projects.DanceChoreographyManager_ApiGateway>("api-gateway")
//    .WithReference(authService)
//    .WithReference(dancerService)
//    .WithReference(courseService)
//    .WithEndpoint(name: "api", port: 5000);

// TODO: Add Frontend project when ready
// var frontend = builder.AddNpmApp("frontend", "../Frontend", "dev")
//    .WithEndpoint(name: "frontend", port: 5173, scheme: "http");

await builder.Build().RunAsync();