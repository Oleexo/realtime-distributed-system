using AutoMapper;
using Oleexo.RealtimeDistributedSystem.Store.Api.Models.Requests;
using Oleexo.RealtimeDistributedSystem.Store.Api.Models.Responses;
using Oleexo.RealtimeDistributedSystem.Store.Commands.EditMessage;

namespace Oleexo.RealtimeDistributedSystem.Store.Api.Models.Mappings;

public sealed class EditMessageMapping : Profile {
    public EditMessageMapping() {
        CreateMap<EditMessageRequest, EditMessageCommand>();
        CreateMap<long, EditMessageResponse>()
           .ForMember(p => p.MessageId, dst => dst.MapFrom(x => x));
    }
}