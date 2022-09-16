using AutoMapper;
using Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Requests;
using Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Responses;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.DispatchMessage;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Mappings;

public sealed class DispatchMessageMapping : Profile {
    public DispatchMessageMapping() {
        CreateMap<DispatchMessageRequest, DispatchMessageCommand>();
        CreateMap<long, DispatchMessageResponse>()
           .ForMember(p => p.MessageId, dst => dst.MapFrom(x => x));
    }
}