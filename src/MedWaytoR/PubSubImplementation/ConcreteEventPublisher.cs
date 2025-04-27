using System.Collections.Concurrent;
using MWR.MedWaytoR.PubSub;

namespace MWR.MedWaytoR.PubSubImplementation;

public class EventPublisher : IEventPublisher
{
    internal EventPublisher(EventExecutorsFactory eventExecutorsFactory)
    {
        _eventExecutorsFactory = eventExecutorsFactory;
    }

    private readonly EventExecutorsFactory _eventExecutorsFactory;

    private readonly ConcurrentDictionary<Type, IEventExecutor> _eventExecutors = [];

    public Task PublishAsync(IEvent @event, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(@event);

        return PublishInternalAsync(@event, ct);
    }

    public async Task PublishAsync(IEnumerable<IEvent> events, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(events);

        foreach (var @event in events)
        {
            if (@event == null!) continue;

            await PublishInternalAsync(@event, ct);
        }
    }

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Task PublishInternalAsync(IEvent @event, CancellationToken ct)
    {
        var executor = _eventExecutors.GetOrAdd(@event.GetType(), _eventExecutorsFactory.Create(@event));

        return executor.Execute(@event, ct);
    }
}