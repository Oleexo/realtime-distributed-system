namespace Oleexo.RealtimeDistributedSystem.Pusher.Service;

public record ConnectionId(string Id)
{
    public static ConnectionId New()
    {
        return new ConnectionId(Guid.NewGuid().ToString());
    }
}