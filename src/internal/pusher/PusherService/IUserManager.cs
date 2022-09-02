namespace Oleexo.RealtimeDistributedSystem.Pusher.Service;

public interface IUserManager {
    Task<ConnectionId> ConnectAsync(string            userId,
                                    string            deviceId,
                                    ChannelFilter     filter,
                                    CancellationToken cancellationToken = default);

    Task DisconnectAsync(ConnectionId connectionId);
    Task RefreshAllAsync();
}
