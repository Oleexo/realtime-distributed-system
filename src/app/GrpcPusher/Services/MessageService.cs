using Grpc.Core;
using Oleexo.RealtimeDistributedSystem.Grpc.Pusher.Message;

namespace GrpcPusher.Services;

internal class MessageService : Message.MessageBase
{
    public override Task Listen(ListenRequest request, IServerStreamWriter<ListenReply> responseStream, ServerCallContext context)
    {
        // todo register user online with filter
        return base.Listen(request, responseStream, context);
    }
}