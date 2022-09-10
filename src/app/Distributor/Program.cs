using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using MassTransit;
using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Common.Data.Repositories.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Mappings;
using Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Requests;
using Oleexo.RealtimeDistributedSystem.Distributor.BrokerPusher.Sqs;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.DispatchEvent;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.DispatchMessage;
using Oleexo.RealtimeDistributedSystem.Distributor.Consumers;
using static Oleexo.RealtimeDistributedSystem.Common.AspNetCoreHelpers.HttpHelpers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediatR(typeof(DispatchMessageCommand));
builder.Services.AddDynamoDbPersistence(builder.Configuration);
builder.Services.AddCommonRepositories();
builder.Services.AddAutoMapper(typeof(DispatchMessageMapping));
builder.Services.AddSqsBrokerPusher(builder.Configuration);
builder.Services.AddMassTransit(x => {
    x.AddConsumer<MessageConsumer, MessageConsumerDefinition>();
    x.UsingAmazonSqs((ctx,
                      cfg) => {
        var region = builder.Configuration["Aws:Region"];
        cfg.Host("localhost", h => {
            var cred = new BasicAWSCredentials("dummy", "Dummy");
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

// Post a message
app.MapPost("/message", RunCommandAsync<DispatchMessageRequest, DispatchMessageCommand>);
// Post an event
app.MapPost("/event", RunCommandAsync<DispatchEventRequest, DispatchEventCommand>);
app.Run();
