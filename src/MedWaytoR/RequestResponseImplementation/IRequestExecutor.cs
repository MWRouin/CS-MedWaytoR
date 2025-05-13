using MWR.MedWaytoR.RequestResponse;

namespace MWR.MedWaytoR.RequestResponseImplementation;

internal interface IRequestExecutor
{
    Task<TResponse> Execute<TResponse>(IRequest<TResponse> request, CancellationToken ct = default);
}