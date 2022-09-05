using Grpc.Core;
using Oleexo.RealtimeDistributedSystem.Grpc.Pusher.Message;
using Oleexo.RealtimeDistributedSystem.Pusher.UserManager;
using ChannelFilter = Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects.ChannelFilter;

namespace Oleexo.RealtimeDistributedSystem.GrpcPusher.Api.Services;

internal class MessageService : Message.MessageBase {
    private readonly ILogger<MessageService> _logger;
    private readonly IUserManager            _userManager;

    public MessageService(IUserManager            userManager,
                          ILogger<MessageService> logger) {
        _userManager = userManager;
        _logger      = logger;
    }

    private async Task HandleMessage(IServerStreamWriter<ListenReply>                                responseStream,
                                     Oleexo.RealtimeDistributedSystem.Common.Domain.Entities.Message message,
                                     CancellationToken                                               cancellationToken = default) {
        try {
            var reply = new ListenReply {
                Message = message.Content
            };
            await responseStream.WriteAsync(reply, cancellationToken);
        }
        catch (OperationCanceledException ex) when (ex.CancellationToken == cancellationToken) {
        }
        catch (Exception e) {
            _logger.LogError(e, "Cannot dispatch message to client");
        }
    }

    public override async Task Listen(ListenRequest                    request,
                                      IServerStreamWriter<ListenReply> responseStream,
                                      ServerCallContext                context) {
        var userId = context.RequestHeaders.GetValue("X-UserId");
        if (string.IsNullOrEmpty(userId)) {
            return;
        }

        var connectionId = await _userManager.ConnectAsync(userId,
                                                           request.DeviceId,
                                                           new ChannelFilter {
                                                               Tags = request.Filter.Tags.ToArray()
                                                           },
                                                           m => HandleMessage(responseStream, m, context.CancellationToken));
        if (connectionId is null) {
            return;
        }

        WaitHandle.WaitAny(new[] { context.CancellationToken.WaitHandle });
        await _userManager.DisconnectAsync(connectionId);
    }
}
