namespace MedWaytoR.RequestResponse;

public interface IRequestDispatcher
{
    Task<TResponse> Dispatch<TRequest, TResponse>(TRequest request, CancellationToken ct = default)
        where TRequest : IRequest<TResponse>;
}