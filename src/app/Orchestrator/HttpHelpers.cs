using AutoMapper;
using MediatR;
using Oleexo.RealtimeDistributedSystem.Common.Commands;

namespace Oleexo.RealtimeDistributedSystem.Orchestrator.Api;

public static class HttpHelpers {
    public static Task<IResult> GetAsync<TRequest, TResponse, TQuery, TQueryResponse>(HttpContext httpContext) {
        throw new NotImplementedException();
    }

    public static async Task RunCommandAsync<TRequest, TResponse, TCommand, TCommandResult>(HttpContext httpContext)
        where TCommand : ICommand<TCommandResult> {
        var request  = await httpContext.Request.ReadFromJsonAsync<TRequest>();
        var mapper   = httpContext.RequestServices.GetRequiredService<IMapper>();
        var mediator = httpContext.RequestServices.GetRequiredService<IMediator>();
        var command  = mapper.Map<TCommand>(request);
        var result   = await mediator.Send(command, httpContext.RequestAborted);
        result.Match(r => HandleResponse<TCommandResult, TResponse>(r, mapper, httpContext), e => HandleException(e, httpContext));
    }

    public static async Task RunCommandAsync<TRequest, TCommand>(HttpContext httpContext)
        where TCommand : ICommand {
        var request  = await httpContext.Request.ReadFromJsonAsync<TRequest>();
        var mapper   = httpContext.RequestServices.GetRequiredService<IMapper>();
        var mediator = httpContext.RequestServices.GetRequiredService<IMediator>();
        var command  = mapper.Map<TCommand>(request);
        var result   = await mediator.Send(command, httpContext.RequestAborted);
        result.Match(_ => httpContext.Response.StatusCode = StatusCodes.Status204NoContent,
                     exception => HandleException(exception, httpContext));
    }

    private static void HandleResponse<TCommandResult, TResponse>(TCommandResult result,
                                                                  IMapper        mapper,
                                                                  HttpContext    httpContext) {
        var response = mapper.Map<TResponse>(result);
        httpContext.Response.StatusCode = StatusCodes.Status200OK;
        httpContext.Response.WriteAsJsonAsync(response);
    }

    private static void HandleException(Exception   exception,
                                        HttpContext httpContext) {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
    }
}
