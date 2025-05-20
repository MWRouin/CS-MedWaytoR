namespace MWR.MedWaytoR.RequestResponse;

public interface IRequestHandler<in TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest command, CancellationToken ct);
}