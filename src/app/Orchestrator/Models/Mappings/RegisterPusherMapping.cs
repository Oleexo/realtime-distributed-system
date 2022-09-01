using AutoMapper;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Api.Models.Requests;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Api.Models.Responses;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Commands.RegisterPusher;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Api.Models.Mappings;

public class RegisterPusherMapping : Profile
{
    public RegisterPusherMapping()
    {
        CreateMap<RegisterPusherRequest, RegisterPusherCommand>();
        CreateMap<RegisterPusherResult, RegisterPusherResponse>();
    }
}