namespace MWR.MedWaytoR.RequestResponse;

public interface IRequestDispatcher
{
    /// <summary>
    /// Dispatches the specified request and returns a response asynchronously.
    /// </summary>
    /// <param name="request">The request to be dispatched, implementing IRequest&lt;TResponse&gt;.</param>
    /// <param name="ct">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResponse">The type of response expected from the dispatched request.</typeparam>
    /// <returns></returns>
    Task<TResponse> DispatchAsync<TResponse>(IRequest<TResponse> request, CancellationToken ct = default);
}