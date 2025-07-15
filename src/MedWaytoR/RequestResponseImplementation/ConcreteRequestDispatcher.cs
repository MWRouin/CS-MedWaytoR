using System.Collections.Concurrent;
using MWR.MedWaytoR.RequestResponse;

namespace MWR.MedWaytoR.RequestResponseImplementation;

internal class ConcreteRequestDispatcher(IServiceProvider serviceProvider) : IRequestDispatcher
{
    private static readonly ConcurrentDictionary<Type, IRequestExecutor> RequestExecutors = [];

    public Task<TResponse> DispatchAsync<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var executor = GetExecutorFor(request);

        return executor.Execute(request, serviceProvider, ct);
    }

    private static IRequestExecutor<TResponse> GetExecutorFor<TResponse>(IRequest<TResponse> request)
    {
        var requestType = request.GetType();

        var executor = RequestExecutors.GetOrAdd(requestType, RequestExecutorsFactory.CreateFor);

        return (IRequestExecutor<TResponse>)executor;
    }
}