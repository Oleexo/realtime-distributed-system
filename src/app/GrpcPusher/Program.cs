using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Common.Data.Repositories.DynamoDb;
using Oleexo.RealtimeDistributedSystem.GrpcPusher.Api.Services;
using Oleexo.RealtimeDistributedSystem.Orchestrator.HttpClient;
using Oleexo.RealtimeDistributedSystem.Pusher.Service;
using Oleexo.RealtimeDistributedSystem.Pusher.UserManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddUserManager();
builder.Services.AddUserPresenceSystem(builder.Configuration);
builder.Services.AddDynamoDbPersistence(builder.Configuration);
builder.Services.AddCommonRepositories();
builder.Services.AddUserPresenceHealthCheck();
builder.Services.AddOrchestratorApi(builder.Configuration);
if (builder.Environment.IsDevelopment()) {
    builder.Services.AddGrpcReflection();
}

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.MapGrpcReflectionService();
}
app.MapGrpcService<MessageService>();
app.MapGet("/",
           () =>
               "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.MapHealthChecks("/healthz/ready", new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("ready")
});

app.MapHealthChecks("/healthz/live", new HealthCheckOptions
{
    Predicate = _ => false
});
app.Run();
