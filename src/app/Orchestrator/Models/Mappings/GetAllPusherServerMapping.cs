using AutoMapper;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Domain.Entities;
using Oleexo.RealtimeDistributedSystem.Orchestrator.HttpModels.Requests;
using Oleexo.RealtimeDistributedSystem.Orchestrator.HttpModels.Responses;
using Oleexo.RealtimeDistributedSystem.Orchestrator.Queries.RetrieveMultipleServer;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Api.Models.Mappings;

public class GetAllPusherServerMapping : Profile {
    public GetAllPusherServerMapping() {
        CreateMap<GetAllPusherServerRequest, RetrieveMultipleServerQuery>();
        CreateMap<PusherServer, PusherServerResponse>();
    }
}
