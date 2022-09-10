using AutoMapper;
using Oleexo.RealtimeDistributedSystem.Store.Api.Models.Requests;
using Oleexo.RealtimeDistributedSystem.Store.Queries.GetMessage;

namespace Oleexo.RealtimeDistributedSystem.Store.Api.Models.Mappings;

public sealed class GetMessageMapping : Profile {
    public GetMessageMapping() {
        CreateMap<GetMessageRequest, RetrieveMessageQuery>();
    }
}