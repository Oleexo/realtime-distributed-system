using AutoMapper;
using Oleexo.RealtimeDistributedSystem.Store.Api.Models.Requests;
using Oleexo.RealtimeDistributedSystem.Store.Api.Models.Responses;
using Oleexo.RealtimeDistributedSystem.Store.Commands.DeleteMessage;

namespace Oleexo.RealtimeDistributedSystem.Store.Api.Models.Mappings;

public sealed class DeleteMessageMapping : Profile {
    public DeleteMessageMapping() {
        CreateMap<DeleteMessageRequest, DeleteMessageCommand>();
        CreateMap<long, DeleteMessageResponse>()
           .ForMember(p => p.MessageId, dst => dst.MapFrom(x => x));
    }
}
