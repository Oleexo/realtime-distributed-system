using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Oleexo.RealtimeDistributedSystem.Common.Commands;
using Oleexo.RealtimeDistributedSystem.Common.Queries;

namespace Oleexo.RealtimeDistributedSystem.Common.AspNetCoreHelpers;

public static class HttpHelpers {
    public static async Task RunQueryAsync<TRequest, TResponse, TQuery, TQueryResponse>(HttpContext httpContext)
        where TQuery : IQuery<TQueryResponse> {
        var request = await httpContext.Request.ReadFromJsonAsync<TRequest>();
        if (request is IHttpRequestModel httpRequestModel) {
            httpRequestModel.PopulateFromContext(httpContext.Request);
        }

        var mapper   = httpContext.RequestServices.GetRequiredService<IMapper>();
        var mediator = httpContext.RequestServices.GetRequiredService<IMediator>();
        var command  = mapper.Map<TQuery>(request);
        var result   = await mediator.Send(command, httpContext.RequestAborted);
        result.Match(r => HandleResponse<TQueryResponse, TResponse>(r, mapper, httpContext), e => HandleException(e, httpContext));
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
