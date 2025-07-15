namespace MWR.MedWaytoR.RequestResponse;

public interface IRequestHandler<in TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Interface to be implemented by the user to define the handler's flow.
    /// Handles a request and returns a response asynchronously.
    /// </summary>
    /// <param name="command">The request to handle.</param>
    /// <param name="ct">A cancellation token for the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the response.</returns>
    Task<TResponse> Handle(TRequest command, CancellationToken ct);
}