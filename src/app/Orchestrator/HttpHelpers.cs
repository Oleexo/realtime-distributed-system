using AutoMapper;
using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Commands;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Api;

public static class HttpHelpers
{
    public static Task<IResult> GetAsync<TRequest, TResponse, TQuery, TQueryResponse>(HttpContext httpContext)
    {
        throw new NotImplementedException();
    }
    
    public static async Task<IResult> RunCommandAsync<TRequest, TResponse, TCommand, TCommandResult>(HttpContext httpContext)
    where TCommand : ICommand<TCommandResult>
    {
        var request = await httpContext.Request.ReadFromJsonAsync<TRequest>();
        var mapper = httpContext.RequestServices.GetRequiredService<IMapper>();
        var mediator = httpContext.RequestServices.GetRequiredService<IMediator>();
        var command = mapper.Map<TCommand>(request);
        var result = await mediator.Send(command, httpContext.RequestAborted);
        return result.Match(r => HandleResponse<TCommandResult, TResponse>(r, mapper), HandleException);
    }

    private static IResult HandleResponse<TCommandResult, TResponse>(TCommandResult result, IMapper mapper)
    {
        var response = mapper.Map<TResponse>(result);
        return Results.Ok(response);
    }

    private static IResult HandleException(Exception exception)
    {
        return Results.StatusCode(500);
    }
}