using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Oleexo.RealtimeDistributedSystem.Orchestrator.HttpClient;
using Oleexo.RealtimeDistributedSystem.Orchestrator.HttpModels.Requests;
using Oleexo.RealtimeDistributedSystem.Pusher.BrokerListener;

namespace Oleexo.RealtimeDistributedSystem.Pusher.Service;

internal sealed class MessageListenerHostedService : IHostedService {
    private readonly ServiceOptions   _configuration;
    private readonly IBrokerListener  _brokerListener;
    private readonly IOrchestratorApi _orchestratorApi;

    public MessageListenerHostedService(IOptions<ServiceOptions> options,
                                        IOrchestratorApi         orchestratorApi,
                                        IBrokerListener          brokerListener) {
        _orchestratorApi = orchestratorApi;
        _brokerListener  = brokerListener;
        _configuration   = options.Value;
    }

    private Task ConsumeMessage(string message) {
        return Task.CompletedTask;
    }
    public async Task StartAsync(CancellationToken cancellationToken) {
        var response = await _orchestratorApi.RegisterPusher(new RegisterPusherRequest {
            Name = _configuration.Name
        });

        _brokerListener.Listen(response.QueueType, response.QueueName, ConsumeMessage);
    }

    public async Task StopAsync(CancellationToken cancellationToken) {
        await _brokerListener.StopAsync();
    }
}
