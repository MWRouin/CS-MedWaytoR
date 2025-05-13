using Microsoft.Extensions.DependencyInjection;
using MWR.MedWaytoR.PubSub;

namespace MWR.MedWaytoR.PubSubImplementation;

internal class EventExecutor<TEvent>(
    ExecutionType executionType,
    IServiceProvider serviceProvider) : IEventExecutor
    where TEvent : IEvent
{
    public Task Execute(IEvent @event, CancellationToken ct = default)
    {
        if (@event is not TEvent)
            throw new ArgumentException($"event is not of a valid type {typeof(TEvent).Name}", nameof(@event));

        var eventHandlers = serviceProvider
            .GetServices<IEventSubscriber<TEvent>>()
            .Select(sub => (Func<Task>)(() => sub.Handle((TEvent)@event, ct)));

        return executionType switch
        {
            ExecutionType.Sequential => eventHandlers.RunAllInSequence(ct),
            ExecutionType.Parallel => eventHandlers.RunAllInParallel(),
            _ => throw new InvalidOperationException(
                $"""
                 Execution type {executionType} is not supported.
                 Use {nameof(ExecutionType.Sequential)} or {nameof(ExecutionType.Parallel)}.
                 """)
        };
    }
}