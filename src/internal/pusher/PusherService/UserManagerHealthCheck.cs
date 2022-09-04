using Microsoft.Extensions.Diagnostics.HealthChecks;
using Oleexo.RealtimeDistributedSystem.Pusher.UserManager;

namespace Oleexo.RealtimeDistributedSystem.Pusher.Service; 

public class UserManagerHealthCheck : IHealthCheck {
    private readonly IUserManager _userManager;

    public UserManagerHealthCheck(IUserManager userManager) {
        _userManager = userManager;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
                                                          CancellationToken  cancellationToken = default) {
        var ready = _userManager.IsReady();

        if (ready) {
            return Task.FromResult(HealthCheckResult.Healthy());
        }

        return Task.FromResult(HealthCheckResult.Unhealthy());
    }
}