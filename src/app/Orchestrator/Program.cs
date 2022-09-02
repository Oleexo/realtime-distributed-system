using MediatR;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Api.HostedServices;
using Oleexo.RealtimeDistributedSystem.Orchestrator.BrokerManager.AmazonSqs;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.PusherRefresh;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.RegisterPusher;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.UnregisterPusher;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Data.InMemory;
using Oleexo.RealtimeDistributedSystem.Orchestrator.HttpModels.Requests;
using Oleexo.RealtimeDistributedSystem.Orchestrator.HttpModels.Responses;
using static Oleexo.RealtimeDistributedSystem.Orchestrator.Api.HttpHelpers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddBrokerService(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMediatR(typeof(RegisterPusherCommand));
builder.Services.AddPersistence();
builder.Services.AddHostedService<ServerCleanerHostedService>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/pusher/refresh",  RunCommandAsync<RefreshPusherRequest, RefreshPusherCommand>);
app.MapPost("/pusher/register", RunCommandAsync<RegisterPusherRequest, RegisterPusherResponse, RegisterPusherCommand, RegisterPusherResult>);
app.MapDelete("/pusher/unregister", RunCommandAsync<UnregisterPusherRequest, UnregisterPusherCommand>);

app.Run();
