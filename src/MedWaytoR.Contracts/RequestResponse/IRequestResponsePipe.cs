namespace MWR.MedWaytoR.RequestResponse;

public interface IRequestResponsePipe<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Implement this interface to inject custom behavior (pipeline, middleware, or filter-like logic)
    /// into the request handling flow.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="nextPipe"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<TResponse> Pipe(TRequest request, Func<Task<TResponse>> nextPipe, CancellationToken ct);
}