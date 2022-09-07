using AutoMapper;
using Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Requests;
using Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Responses;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.EditMessage;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Mappings;

public sealed class EditMessageMapping : Profile {
    public EditMessageMapping() {
        CreateMap<EditMessageRequest, EditMessageCommand>();
        CreateMap<long, EditMessageResponse>()
           .ForMember(p => p.MessageId, dst => dst.MapFrom(x => x));
    }
}