using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Common.Data.Repositories.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Mappings;
using Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Requests;
using Oleexo.RealtimeDistributedSystem.Distributor.BrokerPusher.Sqs;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.DispatchEvent;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.DispatchMessage;
using static Oleexo.RealtimeDistributedSystem.Common.AspNetCoreHelpers.HttpHelpers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqsBrokerPusher(builder.Configuration);
builder.Services.AddMediatR(typeof(DispatchMessageCommand));
builder.Services.AddDynamoDbPersistence(builder.Configuration);
builder.Services.AddCommonRepositories();
builder.Services.AddAutoMapper(typeof(DispatchMessageMapping));
var app = builder.Build();

// Post a message
app.MapPost("/message", RunCommandAsync<DispatchMessageRequest, DispatchMessageCommand>);
// Post an event
app.MapPost("/event", RunCommandAsync<DispatchEventRequest, DispatchEventCommand>);
app.Run();
