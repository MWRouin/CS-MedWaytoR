namespace MedWaytoR.RequestResponse;

public interface IRequestResponsePipe<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Pipe(TRequest request, Func<Task<TResponse>> nextPipe, CancellationToken ct);
}