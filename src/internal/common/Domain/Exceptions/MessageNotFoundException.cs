namespace Oleexo.RealtimeDistributedSystem.Common.Domain.Exceptions;

public class MessageNotFoundException : DomainException {
    public MessageNotFoundException(string channelId,
                                    long   messageId)
        : base($"The message {channelId}:{messageId} was not found") {
    }
}
