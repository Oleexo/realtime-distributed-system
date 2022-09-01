using Oleexo.RealtimeDistributedSystem.Orchestrator.Api.Models.Requests;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Api.Models.Responses;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.RegisterPusher;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Services;
using static Oleexo.RealtimeDistributedSystem.Orchestrator.Api.HttpHelpers;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddBrokerService(builder.Configuration);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/pusher/register", RunCommandAsync<RegisterPusherRequest, RegisterPusherResponse, RegisterPusherCommand, RegisterPusherResult>);

app.Run();