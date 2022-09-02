namespace Oleexo.RealtimeDistributedSystem.Pusher.Service;

internal class ConnectionInfo
{
    public ConnectionInfo(string userId, string deviceId, ChannelFilter filter)
    {
        Id       = ConnectionId.New(); 
        UserId   = userId;
        DeviceId = deviceId;
        Filter   = filter;
    }

    public ConnectionId  Id       { get; }
    public string        UserId   { get; }
    public string        DeviceId { get; }
    public ChannelFilter Filter   { get; }
}