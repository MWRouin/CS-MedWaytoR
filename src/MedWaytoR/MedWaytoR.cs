using Microsoft.Extensions.DependencyInjection;
using MWR.MedWaytoR.PubSub;
using MWR.MedWaytoR.RequestResponse;

namespace MWR.MedWaytoR;

/// <summary>
/// The main entry point for MedWaytoR.
/// Delegates the request and event handling to the appropriate services.
/// </summary>
/// <param name="serviceProvider"></param>
internal class MedWaytoR(IServiceProvider serviceProvider) : IMedWaytoR
{
    private readonly Lazy<IRequestDispatcher> _requestDispatcher =
        new(serviceProvider.GetRequiredService<IRequestDispatcher>);

    private readonly Lazy<IEventPublisher> _eventPublisher =
        new(serviceProvider.GetRequiredService<IEventPublisher>);

    public Task<TResponse> DispatchAsync<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
    {
        return _requestDispatcher.Value.DispatchAsync(request, ct);
    }

    public Task PublishAsync(IEvent @event, CancellationToken ct = default)
    {
        return _eventPublisher.Value.PublishAsync(@event, ct);
    }

    public Task PublishAsync(IEnumerable<IEvent> events, CancellationToken ct = default)
    {
        return _eventPublisher.Value.PublishAsync(events, ct);
    }
}