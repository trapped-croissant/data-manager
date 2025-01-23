using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    var username = builder.AddParameter("StateStoreUser", secret: true);
    var password = builder.AddParameter("StateStorePassword", secret: true);
    var postgres = builder.AddPostgres("postgres", username, password, 5432)
        .WithLifetime(ContainerLifetime.Persistent);
}

var redis = builder.AddRedis("redis")
    .WithRedisCommander();

var dataGenerator = builder.AddProject<Projects.Carr_DataGenerator>("carr-data-generator")
    .WaitFor(redis)
    .WithReference(redis);

builder.AddNpmApp("react", "../data-manager-front-end")
    .WithReference(dataGenerator)
    .WaitFor(dataGenerator)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpsEndpoint(targetPort: 4001, port: 3000, name: "react-front-end")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();