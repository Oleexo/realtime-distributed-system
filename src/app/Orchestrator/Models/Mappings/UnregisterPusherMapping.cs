using AutoMapper;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.UnregisterPusher;
using Oleexo.RealtimeDistributedSystem.Orchestrator.HttpModels.Requests;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Api.Models.Mappings;

public class UnregisterPusherMapping : Profile {
    public UnregisterPusherMapping() {
        CreateMap<UnregisterPusherRequest, UnregisterPusherCommand>();
    }
}
