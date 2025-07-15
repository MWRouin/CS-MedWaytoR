using Microsoft.Extensions.DependencyInjection;
using MWR.MedWaytoR.RequestResponse;

namespace MWR.MedWaytoR.RequestResponseImplementation;

public class RequestExecutor<TRequest, TResponse>
    : IRequestExecutor<TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Execute(
        IRequest<TResponse> request,
        IServiceProvider serviceProvider,
        CancellationToken ct = default)
    {
        if (request is not TRequest typedRequest)
            throw new ArgumentException(
                $"Invalid request type. Expected {typeof(TRequest).Name}, got {request?.GetType().Name ?? "null"}",
                nameof(request));

        var executionPipeLineFunc = BuildExecutionPipeLine(typedRequest, serviceProvider, ct);

        return await executionPipeLineFunc().ConfigureAwait(false);
    }

    private static Func<Task<TResponse>> BuildExecutionPipeLine(
        TRequest request,
        IServiceProvider serviceProvider,
        CancellationToken ct)
    {
        if (ct.IsCancellationRequested) return () => Task.FromCanceled<TResponse>(ct);

        var handler = serviceProvider.GetServices<IRequestHandler<TRequest, TResponse>>().Single();

        var pipesEnumerable = serviceProvider.GetServices<IRequestResponsePipe<TRequest, TResponse>>();

        var pipeline = pipesEnumerable.Reverse() // Reverse to wrap in correct order: outer → inner
            .Aggregate(
                (Func<Task<TResponse>>)(() => handler.Handle(request, ct)),
                (next, pipe) => () => ct.IsCancellationRequested
                    ? Task.FromCanceled<TResponse>(ct)
                    : pipe.Pipe(request, next, ct));

        return pipeline;
    }
}