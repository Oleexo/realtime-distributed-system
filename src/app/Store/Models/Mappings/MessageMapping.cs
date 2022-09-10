using AutoMapper;
using Oleexo.RealtimeDistributedSystem.Common.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Store.Api.Models.Responses;

namespace Oleexo.RealtimeDistributedSystem.Store.Api.Models.Mappings;

public sealed class MessageMapping : Profile {
    public MessageMapping() {
        CreateMap<Message, MessageResponse>()
           .ForMember(p => p.MessageId,  dst => dst.MapFrom(x => x.Id))
           .ForMember(p => p.Content,    dst => dst.MapFrom(x => x.Content))
           .ForMember(p => p.ChannelId,  dst => dst.MapFrom(x => x.ChannelId))
           .ForMember(p => p.ParentId,   dst => dst.MapFrom(x => x.ParentId))
           .ForMember(p => p.IsDeletion, dst => dst.MapFrom(x => x.IsDeletion));
    }
}