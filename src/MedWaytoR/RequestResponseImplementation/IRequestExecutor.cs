using MWR.MedWaytoR.RequestResponse;

namespace MWR.MedWaytoR.RequestResponseImplementation;

internal interface IRequestExecutor;

internal interface IRequestExecutor<TResponse> : IRequestExecutor
{
    Task<TResponse> Execute(
        IRequest<TResponse> request, 
        IServiceProvider serviceProvider,
        CancellationToken ct = default);
}