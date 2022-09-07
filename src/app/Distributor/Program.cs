using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Common.Data.Repositories.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Mappings;
using Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Requests;
using Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Responses;
using Oleexo.RealtimeDistributedSystem.Distributor.BrokerPusher.Sqs;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.DeleteMessage;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.DispatchEvent;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.DispatchMessage;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.EditMessage;
using Oleexo.RealtimeDistributedSystem.Distributor.SnowflakeGen;
using static Oleexo.RealtimeDistributedSystem.Common.AspNetCoreHelpers.HttpHelpers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqsBrokerPusher(builder.Configuration);
builder.Services.AddMediatR(typeof(DispatchMessageCommand));
builder.Services.AddDynamoDbPersistence(builder.Configuration);
builder.Services.AddCommonRepositories();
builder.Services.AddAutoMapper(typeof(DispatchMessageMapping));
builder.Services.AddSnowflakeGen();
var app = builder.Build();

// Post a message
app.MapPost("/message", RunCommandAsync<DispatchMessageRequest, DispatchMessageResponse, DispatchMessageCommand, long>);
// Post an event
app.MapPost("/event", RunCommandAsync<DispatchEventRequest, DispatchEventCommand>);
// Edit message
app.MapPut("/message", RunCommandAsync<EditMessageRequest, EditMessageResponse, EditMessageCommand, long>);
// Delete message
app.MapDelete("/message", RunCommandAsync<DeleteMessageRequest, DeleteMessageResponse, DeleteMessageCommand, long>);
app.Run();
