using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Oleexo.RealtimeDistributedSystem.Common.AspNetCoreHelpers;

public static class WebApplicationExtensions {
    public static WebApplication MapGcCollectDebug(this WebApplication app,
                                                   string              path = "/debug/gc") {
        app.MapGet(path, () => {
            GC.Collect();
            return Results.NoContent();
        });
        return app;
    }
}
