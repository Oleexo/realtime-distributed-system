using AutoMapper;
using Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Requests;
using Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Responses;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.DeleteMessage;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Mappings;

public sealed class DeleteMessageMapping : Profile {
    public DeleteMessageMapping() {
        CreateMap<DeleteMessageRequest, DeleteMessageCommand>();
        CreateMap<long, DeleteMessageResponse>()
           .ForMember(p => p.MessageId, dst => dst.MapFrom(x => x));
    }
}
