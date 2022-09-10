using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.AspNetCoreHelpers;
using Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Common.Data.Repositories.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Store.Api.Models.Mappings;
using Oleexo.RealtimeDistributedSystem.Store.Api.Models.Requests;
using Oleexo.RealtimeDistributedSystem.Store.Api.Models.Responses;
using Oleexo.RealtimeDistributedSystem.Store.Commands.DeleteMessage;
using Oleexo.RealtimeDistributedSystem.Store.Commands.EditMessage;
using Oleexo.RealtimeDistributedSystem.Store.Queries.RetrieveMessage;
using Oleexo.RealtimeDistributedSystem.Store.SnowflakeGen;
using static Oleexo.RealtimeDistributedSystem.Common.AspNetCoreHelpers.HttpHelpers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediatR(typeof(RetrieveMessageQuery));
builder.Services.AddDynamoDbPersistence(builder.Configuration);
builder.Services.AddCommonRepositories();
builder.Services.AddAutoMapper(typeof(GetMessageMapping));
builder.Services.AddSnowflakeGen();


var app     = builder.Build();

app.MapGet("/message", RunQueryAsync<GetMessageRequest, MessageResponse, RetrieveMessageQuery, Message>);
// Edit message
app.MapPut("/message", RunCommandAsync<EditMessageRequest, EditMessageResponse, EditMessageCommand, long>);
// Delete message
app.MapDelete("/message", RunCommandAsync<DeleteMessageRequest, DeleteMessageResponse, DeleteMessageCommand, long>);

app.MapGcCollectDebug();

app.Run();