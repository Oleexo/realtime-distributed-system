using AutoMapper;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.PusherRefresh;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.RegisterPusher;
using Oleexo.RealtimeDistributedSystem.Orchestrator.HttpModels.Requests;
using Oleexo.RealtimeDistributedSystem.Orchestrator.HttpModels.Responses;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Api.Models.Mappings;

public class RegisterPusherMapping : Profile {
    public RegisterPusherMapping() {
        CreateMap<RegisterPusherRequest, RegisterPusherCommand>();
        CreateMap<RegisterPusherResult, RegisterPusherResponse>()
           .ForMember(p => p.QueueType, 
                      dst => dst.MapFrom(x=> x.QueueType.ToString()));
    }
}

public class RefreshPusherMapping : Profile {
    public RefreshPusherMapping() {
        CreateMap<RefreshPusherRequest, RefreshPusherCommand>();
    }
}