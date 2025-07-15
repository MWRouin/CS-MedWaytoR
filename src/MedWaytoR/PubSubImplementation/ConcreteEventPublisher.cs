using System.Collections.Concurrent;
using MWR.MedWaytoR.Config;
using MWR.MedWaytoR.PubSub;

namespace MWR.MedWaytoR.PubSubImplementation;

internal class ConcreteEventPublisher(IServiceProvider sp, PubSubExecutorFunc executorFunc)
    : IEventPublisher
{
    private static readonly ConcurrentDictionary<Type, IEventExecutor> EventExecutors = [];

    /// <inheritdoc cref="IEventPublisher.PublishAsync(IEvent,System.Threading.CancellationToken)"/>
    public Task PublishAsync(IEvent @event, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(@event);

        var executor = GetOrAddExecutor(@event);

        return executor.Execute(@event, sp, executorFunc.ExecuteFunc, ct);
    }

    // supports parallel execution of events switching on MWR.MedWaytoR.PubSubImplementation.ExecutionType
    /// <inheritdoc cref="IEventPublisher.PublishAsync(System.Collections.Generic.IEnumerable{IEvent},System.Threading.CancellationToken)"/>
    public async Task PublishAsync(IEnumerable<IEvent> events, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(events);

        foreach (var @event in events)
        {
            ct.ThrowIfCancellationRequested();

            await PublishAsync(@event, ct);
        }
    }

    private static IEventExecutor GetOrAddExecutor(IEvent @event)
    {
        var eventType = @event.GetType();

        return EventExecutors.GetOrAdd(eventType, EventExecutorsFactory.CreateFor);
    }
}