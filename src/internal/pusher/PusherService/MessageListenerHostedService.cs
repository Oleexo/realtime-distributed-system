using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Common.Domain.ValueObjects;
using Oleexo.RealtimeDistributedSystem.Orchestrator.HttpClient;
using Oleexo.RealtimeDistributedSystem.Orchestrator.HttpModels.Requests;
using Oleexo.RealtimeDistributedSystem.Pusher.BrokerListener;
using Oleexo.RealtimeDistributedSystem.Pusher.UserManager;

namespace Oleexo.RealtimeDistributedSystem.Pusher.Service;

internal sealed class MessageListenerHostedService : IHostedService, IDisposable {
    private readonly IBrokerListener                       _brokerListener;
    private readonly ServiceOptions                        _configuration;
    private readonly IOrchestratorApi                      _orchestratorApi;
    private readonly IUserManager                          _userManager;
    private readonly ILogger<MessageListenerHostedService> _logger;
    private          Timer?                                _timer;

    public MessageListenerHostedService(IOptions<ServiceOptions>              options,
                                        IOrchestratorApi                      orchestratorApi,
                                        IBrokerListener                       brokerListener,
                                        IUserManager                          userManager,
                                        ILogger<MessageListenerHostedService> logger) {
        _orchestratorApi = orchestratorApi;
        _brokerListener  = brokerListener;
        _userManager     = userManager;
        _logger          = logger;
        _configuration   = options.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken) {
        var response = await _orchestratorApi.RegisterPusher(new RegisterPusherRequest {
            Name = _configuration.Name
        });
        _userManager.SetQueueInfo(new QueueInfo(response.QueueType, response.QueueName));
        _brokerListener.Listen(response.QueueType, response.QueueName, ConsumeMessage);
        _timer = new Timer(KeepQueueSlotActivate, null, TimeSpan.FromSeconds(5),
                           TimeSpan.FromSeconds(30));
    }

    private void KeepQueueSlotActivate(object? state) {
        try {
            var request = new RefreshPusherRequest {
                Name = _configuration.Name
            };
            _orchestratorApi.RefreshPusher(request)
                            .GetAwaiter()
                            .GetResult();
            _logger.LogDebug("Keep queue slop activate success");
        }
        catch (Exception e) {
            _logger.LogError(e, "Keep queue slot activate failed");
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken) {
        _timer?.Change(Timeout.Infinite, 0);
        await _brokerListener.StopAsync();
        var request = new UnregisterPusherRequest {
            Name = _configuration.Name
        };
        await _orchestratorApi.UnregisterPusher(request);
    }

    private Task ConsumeMessage(MessageWrapper message) {
        return _userManager.DispatchAsync(message);
    }

    public void Dispose() {
        _timer?.Dispose();
    }
}
