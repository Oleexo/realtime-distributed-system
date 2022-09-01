using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Entities;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Services;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddBrokerService(this IServiceCollection services,
                                                      IConfiguration          configuration) {
        var brokerType = configuration["Broker:Type"];
        if (string.IsNullOrWhiteSpace(brokerType)) {
            throw new InvalidOperationException("Missing broker type");
        }

        if (!Enum.TryParse<QueueType>(brokerType, out var queueType)) {
            throw new InvalidOperationException("Invalid broker type");
        }

        switch (queueType) {
            case QueueType.Sqs:
                services.Configure<SqsOptions>(configuration.GetSection("Broker"));
                services.AddSingleton<IBrokerService, SqsBrokerService>();
                break;
            case QueueType.RabbitMq:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return services;
    }
}