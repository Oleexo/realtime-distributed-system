using AutoMapper;
using Distributor.Models.Requests;
using Distributor.Models.Responses;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.DispatchMessage;

namespace Distributor.Models.Mappings; 

public class DispatchMessageMapping : Profile {
    public DispatchMessageMapping() {
        CreateMap<DispatchMessageRequest, DispatchMessageCommand>();
        CreateMap<long, DispatchMessageResponse>()
           .ForMember(p => p.MessageId, dst => dst.MapFrom(x => x));
    }
}
