using Microsoft.Extensions.DependencyInjection;
using MWR.MedWaytoR.RequestResponse;

namespace MWR.MedWaytoR.RequestResponseImplementation;

public class RequestExecutor<TRequest, TInternalResponse>(
    IServiceProvider serviceProvider) : IRequestExecutor
    where TRequest : IRequest<TInternalResponse>
{
    private async Task<object> InternalExecute(TRequest request, CancellationToken ct = default)
    {
        var requestHandler = serviceProvider
            .GetRequiredService<IRequestHandler<TRequest, TInternalResponse>>();

        var requestResponsePipes = serviceProvider
            .GetServices<IRequestResponsePipe<TRequest, TInternalResponse>>();

        var pipeFunc = () => requestHandler.Handle(request, ct);

        foreach (var pipe in requestResponsePipes) // TODO: check if the order of the pipes is correct
        {
            var nextPipe = pipeFunc;
            pipeFunc = () => pipe.Pipe(request, nextPipe, ct);
        }

        return (await pipeFunc())!;
    }

    public async Task<TResponse> Execute<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
    {
        if (request is not TRequest typedRequest) // also checks for TResponse
        {
            throw new InvalidOperationException(
                $"The request type {request.GetType()} is not compatible with the executor type {typeof(TRequest)}.");
        }

        return (TResponse)await InternalExecute(typedRequest, ct);
    }
}