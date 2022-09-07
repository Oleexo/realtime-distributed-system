using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Oleexo.RealtimeDistributedSystem.Common.Data.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Common.Data.Repositories.DynamoDb;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;
using Oleexo.RealtimeDistributedSystem.Orchestrator.HttpClient;
using Oleexo.RealtimeDistributedSystem.Pusher.Service;
using Oleexo.RealtimeDistributedSystem.Pusher.UserManager;
using Oleexo.RealtimeDistributedSystem.WebPusher.Api.Models.Requests;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddUserManager();
builder.Services.AddUserPresenceSystem(builder.Configuration);
builder.Services.AddDynamoDbPersistence(builder.Configuration);
builder.Services.AddCommonRepositories();
builder.Services.AddUserPresenceHealthCheck();
builder.Services.AddOrchestratorApi(builder.Configuration);

var app = builder.Build();

static async Task HandleMessage(WebSocket webSocket,
                                Message   message) {
    var json  = JsonSerializer.Serialize(message);
    var bytes = Encoding.UTF8.GetBytes(json);
    await webSocket.SendAsync(bytes, WebSocketMessageType.Text, WebSocketMessageFlags.EndOfMessage, CancellationToken.None);
}

static async Task HandleEvent(WebSocket webSocket,
                              Event     @event) {
    var json  = JsonSerializer.Serialize(@event);
    var bytes = Encoding.UTF8.GetBytes(json);
    await webSocket.SendAsync(bytes, WebSocketMessageType.Text, WebSocketMessageFlags.EndOfMessage, CancellationToken.None);
}

app.UseWebSockets();
app.Use(async (context,
               next) => {
    if (context.Request.Path == "/ws" &&
        context.WebSockets.IsWebSocketRequest) {
        var webSocket     = await context.WebSockets.AcceptWebSocketAsync();
        var userManager   = context.RequestServices.GetRequiredService<IUserManager>();
        var buffer        = new byte[1024 * 4];
        var receiveResult = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
        if (receiveResult.CloseStatus.HasValue) {
            return;
        }

        WebSocketRequest? request;
        if (receiveResult.MessageType == WebSocketMessageType.Text) {
            var message = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
            request = JsonSerializer.Deserialize<WebSocketRequest>(message);
            if (request is null) {
                return;
            }
        }
        else {
            return;
        }

        var socketFinishedTcs = new TaskCompletionSource<object>();
        var connectionId = await userManager.ConnectAsync(request.UserId, request.DeviceId, new ChannelFilter {
                                                              Tags = request.Tags.ToArray()
                                                          }, m => HandleMessage(webSocket, m),
                                                          e => HandleEvent(webSocket, e));
        if (connectionId is null) {
            return;
        }

        await socketFinishedTcs.Task;
        await userManager.DisconnectAsync(connectionId);
        webSocket.Dispose();
    }
    else {
        await next(context);
    }
});
app.MapGet("/", () => "Hello World!");

app.Run();
