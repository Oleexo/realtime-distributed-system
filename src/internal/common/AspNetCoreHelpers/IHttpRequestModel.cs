using Microsoft.AspNetCore.Http;

namespace Oleexo.RealtimeDistributedSystem.Common.AspNetCoreHelpers;

public interface IHttpRequestModel {
    void PopulateFromContext(HttpRequest httpRequest);
}
