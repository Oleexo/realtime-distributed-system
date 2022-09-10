using AutoMapper;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Store.Api.Models.Requests;
using Oleexo.RealtimeDistributedSystem.Store.Api.Models.Responses;
using Oleexo.RealtimeDistributedSystem.Store.Queries.GetMessage;

namespace Oleexo.RealtimeDistributedSystem.Store.Api.Models.Mappings;

public sealed class GetMessageMapping : Profile {
    public GetMessageMapping() {
        CreateMap<GetMessageRequest, RetrieveMessageQuery>();
        CreateMap<Message, GetMessageResponse>()
           .ForMember(p => p.MessageId,  dst => dst.MapFrom(x => x.Id))
           .ForMember(p => p.Content,    dst => dst.MapFrom(x => x.Content))
           .ForMember(p => p.ChannelId,  dst => dst.MapFrom(x => x.ChannelId))
           .ForMember(p => p.ParentId,   dst => dst.MapFrom(x => x.ParentId))
           .ForMember(p => p.IsDeletion, dst => dst.MapFrom(x => x.IsDeletion));
    }
}
