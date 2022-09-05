using Distributor.Models.Mappings;
using Distributor.Models.Requests;
using Distributor.Models.Responses;
using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Common.Data.Repositories.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Distributor.BrokerPusher.Sqs;
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
app.MapPost("/message", RunCommandAsync<DispatchMessageRequest, DispatchMessageResponse, DispatchMessageCommand, long>);
// Post an event
app.MapPost("/event", () => "Hello world");
// Edit message
app.MapPut("/message/:channel/:messageId", () => "Hello world");
// Delete message
app.MapDelete("/message/:channel/:messageId", () => "Hello world");
app.Run();
