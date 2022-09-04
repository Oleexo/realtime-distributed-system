using GrpcPusher.Services;
using Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Common.Data.Repositories.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Orchestrator.HttpClient;
using Oleexo.RealtimeDistributedSystem.Pusher.Service;
using Oleexo.RealtimeDistributedSystem.Pusher.UserManager;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddUserManager();
builder.Services.AddUserPresenceSystem(builder.Configuration);
builder.Services.AddDynamoDbPersistence(builder.Configuration);
builder.Services.AddCommonRepositories();
builder.Services.AddUserPresenceHealthCheck();
builder.Services.AddOrchestratorApi(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<MessageService>();
app.MapGet("/",
           () =>
               "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
