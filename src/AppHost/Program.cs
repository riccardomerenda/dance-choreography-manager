var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.DanceChoreographyManager_Services_Authentication>("authentication");

builder.Build().Run();
