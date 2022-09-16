using AutoMapper;
using Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Requests;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.ChannelTyping;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Mappings;

public sealed class ChannelTypingMapping : Profile{
    public ChannelTypingMapping() {
        CreateMap<ChannelTypingRequest, ChannelTypingCommand>();
    }
}
