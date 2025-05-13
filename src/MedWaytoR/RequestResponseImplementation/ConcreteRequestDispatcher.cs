using System.Collections.Concurrent;
using MWR.MedWaytoR.RequestResponse;

namespace MWR.MedWaytoR.RequestResponseImplementation;

internal class ConcreteRequestDispatcher : IRequestDispatcher
{
    internal ConcreteRequestDispatcher(RequestExecutorsFactory requestExecutorsFactory)
    {
        _requestExecutorsFactory = requestExecutorsFactory;
    }

    private readonly RequestExecutorsFactory _requestExecutorsFactory;

    private readonly ConcurrentDictionary<Type, IRequestExecutor> _requestExecutors = [];

    public Task<TResponse> DispatchAsync<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
    {
        var executor = _requestExecutors.GetOrAdd(request.GetType(), _requestExecutorsFactory.Create(request));

        return executor.Execute(request, ct);
    }
}