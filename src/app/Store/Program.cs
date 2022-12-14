using Amazon.SimpleNotificationService;
using Amazon.SQS;
using MassTransit;
using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.AspNetCoreHelpers;
using Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Common.Data.Repositories.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.StartupTasks;
using Oleexo.RealtimeDistributedSystem.Store.Api.Models.Mappings;
using Oleexo.RealtimeDistributedSystem.Store.Api.Models.Requests;
using Oleexo.RealtimeDistributedSystem.Store.Api.Models.Responses;
using Oleexo.RealtimeDistributedSystem.Store.Commands.DeleteMessage;
using Oleexo.RealtimeDistributedSystem.Store.Commands.EditMessage;
using Oleexo.RealtimeDistributedSystem.Store.Commands.ReadMessage;
using Oleexo.RealtimeDistributedSystem.Store.Commands.StoreMessage;
using Oleexo.RealtimeDistributedSystem.Store.Publishers;
using Oleexo.RealtimeDistributedSystem.Store.Queries.RetrieveMessage;
using Oleexo.RealtimeDistributedSystem.Store.SnowflakeGen;
using Prometheus;
using static Oleexo.RealtimeDistributedSystem.Common.AspNetCoreHelpers.HttpHelpers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediatR(typeof(RetrieveMessageQueryHandler), typeof(StoreMessageCommandHandler), typeof(MessageCreatedHandler));
builder.Services.AddDynamoDbPersistence(builder.Configuration);
builder.Services.AddCommonRepositories();
builder.Services.AddAutoMapper(typeof(GetMessageMapping));
builder.Services.AddSnowflakeGen();
builder.Services.AddStartupTasks();
builder.Services.AddMassTransit(x => {
    x.SetKebabCaseEndpointNameFormatter();
    x.UsingAmazonSqs((ctx,
                      cfg) => {
        var region = builder.Configuration["Aws:Region"];
        cfg.Host("localhost", h => {
            h.Config(new AmazonSQSConfig {
                ServiceURL = region
            });
            h.Config(new AmazonSimpleNotificationServiceConfig {
                ServiceURL = region
            });
            h.AccessKey("dummy");
            h.SecretKey("dummy");
        });
        cfg.ConfigureEndpoints(ctx);
    });
});

var app = builder.Build();

app.UseHttpMetrics();
app.MapGet("/message", RunQueryAsync<GetMessageRequest, MessageResponse, RetrieveMessageQuery, Message>);
// Create message
app.MapPost("/message", RunCommandAsync<CreateMessageRequest, CreateMessageResponse, StoreMessageCommand, long>);
// Edit message
app.MapPut("/message", RunCommandAsync<EditMessageRequest, EditMessageResponse, EditMessageCommand, long>);
// Delete message
app.MapDelete("/message", RunCommandAsync<DeleteMessageRequest, DeleteMessageResponse, DeleteMessageCommand, long>);
app.MapPost("/channel/read", RunCommandAsync<MessageReadRequest, ReadMessageCommand>);

app.MapGcCollectDebug();
app.UseMetricServer();

app.Run();
