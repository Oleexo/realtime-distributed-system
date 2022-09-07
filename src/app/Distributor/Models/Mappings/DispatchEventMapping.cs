using AutoMapper;
using Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Requests;
using Oleexo.RealtimeDistributedSystem.Distributor.Commands.DispatchEvent;

namespace Oleexo.RealtimeDistributedSystem.Distributor.Api.Models.Mappings;

public sealed class DispatchEventMapping : Profile {
    public DispatchEventMapping() {
        CreateMap<DispatchEventRequest, DispatchEventCommand>();
    }
}