using AutoMapper;
using Oleexo.RealtimeDistributedSystem.Store.Api.Models.Requests;
using Oleexo.RealtimeDistributedSystem.Store.Api.Models.Responses;
using Oleexo.RealtimeDistributedSystem.Store.Commands.StoreMessage;

namespace Oleexo.RealtimeDistributedSystem.Store.Api.Models.Mappings;

public sealed class CreateMessageMapping : Profile {
    public CreateMessageMapping() {
        CreateMap<CreateMessageRequest, StoreMessageCommand>();
        CreateMap<long, CreateMessageResponse>()
           .ForMember(p => p.MessageId, dst => dst.MapFrom(x => x));
    }
}
