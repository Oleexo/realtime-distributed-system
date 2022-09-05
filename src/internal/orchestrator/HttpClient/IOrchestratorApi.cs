using Oleexo.RealtimeDistributedSystem.Orchestrator.HttpModels.Requests;
using Oleexo.RealtimeDistributedSystem.Orchestrator.HttpModels.Responses;
using Refit;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.HttpClient;

public interface IOrchestratorApi {
    [Post("/pusher/register")]
    public Task<RegisterPusherResponse> RegisterPusher(RegisterPusherRequest request);

    [Post("/pusher/unregister")]
    public Task UnregisterPusher(UnregisterPusherRequest request);

    [Post("/pusher/refresh")]
    public Task RefreshPusher(RefreshPusherRequest request);
}