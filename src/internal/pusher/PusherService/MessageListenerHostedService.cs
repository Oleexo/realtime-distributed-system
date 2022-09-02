using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;
using Oleexo.RealtimeDistributedSystem.Orchestrator.HttpClient;
using Oleexo.RealtimeDistributedSystem.Orchestrator.HttpModels.Requests;
using Oleexo.RealtimeDistributedSystem.Pusher.BrokerListener;
using Oleexo.RealtimeDistributedSystem.Pusher.UserManager;

namespace Oleexo.RealtimeDistributedSystem.Pusher.Service;

internal sealed class MessageListenerHostedService : IHostedService {
    private readonly IBrokerListener  _brokerListener;
    private readonly ServiceOptions   _configuration;
    private readonly IOrchestratorApi _orchestratorApi;
    private readonly IUserManager     _userManager;

    public MessageListenerHostedService(IOptions<ServiceOptions> options,
                                        IOrchestratorApi         orchestratorApi,
                                        IBrokerListener          brokerListener,
                                        IUserManager             userManager) {
        _orchestratorApi = orchestratorApi;
        _brokerListener  = brokerListener;
        _userManager     = userManager;
        _configuration   = options.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken) {
        var response = await _orchestratorApi.RegisterPusher(new RegisterPusherRequest {
            Name = _configuration.Name
        });
        _userManager.SetQueueInfo(new QueueInfo(response.QueueType, response.QueueName));
        _brokerListener.Listen(response.QueueType, response.QueueName, ConsumeMessage);
    }

    public async Task StopAsync(CancellationToken cancellationToken) {
        await _brokerListener.StopAsync();
    }

    private Task ConsumeMessage(MessageWrapper message) {
        return _userManager.DispatchAsync(message);
    }
}
