using AutoMapper;
using Oleexo.RealtimeDistributedSystem.Store.Api.Models.Requests;
using Oleexo.RealtimeDistributedSystem.Store.Queries.RetrieveMultipleMessage;

namespace Oleexo.RealtimeDistributedSystem.Store.Api.Models.Mappings;

public sealed class GetAllMessageMapping : Profile {
    public GetAllMessageMapping() {
        CreateMap<GetAllMessageRequest, RetrieveMultipleMessageQuery>();
    }
}
