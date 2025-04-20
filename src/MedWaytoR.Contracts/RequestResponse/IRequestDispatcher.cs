namespace MedWaytoR.RequestResponse;

public interface IRequestDispatcher
{
    Task<TResponse> DispatchAsync<TRequest, TResponse>(TRequest request, CancellationToken ct = default)
        where TRequest : IRequest<TResponse>;
}