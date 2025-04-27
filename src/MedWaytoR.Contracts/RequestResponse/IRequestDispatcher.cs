namespace MWR.MedWaytoR.RequestResponse;

public interface IRequestDispatcher
{
    Task<TResponse> DispatchAsync<TResponse>(IRequest<TResponse> request, CancellationToken ct = default);
}