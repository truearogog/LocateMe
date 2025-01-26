IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.LocateMe_Api>("locateme-api");

await builder.Build().RunAsync().ConfigureAwait(false);
