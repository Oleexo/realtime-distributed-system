using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Common.StartupTasks;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Api.HostedServices;
using Oleexo.RealtimeDistributedSystem.Orchestrator.BrokerManager.AmazonSqs;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.PusherRefresh;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.RegisterPusher;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.UnregisterPusher;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Data.Repositories.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain;
using Oleexo.RealtimeDistributedSystem.Orchestrator.HttpModels.Requests;
using Oleexo.RealtimeDistributedSystem.Orchestrator.HttpModels.Responses;
using static Oleexo.RealtimeDistributedSystem.Common.AspNetCoreHelpers.HttpHelpers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqsBrokerService(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMediatR(typeof(RegisterPusherCommand));
builder.Services.AddDynamoDbPersistence(builder.Configuration);
builder.Services.AddPersistence();
builder.Services.AddHostedService<ServerCleanerHostedService>();
builder.Services.AddStartupTasks();
builder.Services.Configure<ServiceOptions>(builder.Configuration.GetSection("Orchestrator"));
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/pusher/refresh",    RunCommandAsync<RefreshPusherRequest, RefreshPusherCommand>);
app.MapPost("/pusher/register",   RunCommandAsync<RegisterPusherRequest, RegisterPusherResponse, RegisterPusherCommand, RegisterPusherResult>);
app.MapPost("/pusher/unregister", RunCommandAsync<UnregisterPusherRequest, UnregisterPusherCommand>);


app.MapHealthChecks("/healthz/ready", new HealthCheckOptions {
    Predicate = healthCheck => healthCheck.Tags.Contains("ready")
});

app.MapHealthChecks("/healthz/live", new HealthCheckOptions {
    Predicate = _ => false
});
app.Run();
