using Microsoft.Extensions.DependencyInjection;
using MWR.MedWaytoR.PubSub;

namespace MWR.MedWaytoR.PubSubImplementation;

internal class EventExecutor<TEvent> : IEventExecutor
    where TEvent : IEvent
{
    public Task Execute(
        IEvent @event,
        IServiceProvider serviceProvider,
        Func<IEnumerable<Func<Task>>, CancellationToken, Task> handlersExecutor,
        CancellationToken ct = default)
    {
        if (@event is not TEvent typedEvent)
            throw new ArgumentException(
                $"Invalid event type. Expected {typeof(TEvent).Name}, got {@event?.GetType().Name ?? "null"}",
                nameof(@event));

        var handlers = serviceProvider.GetServices<IEventSubscriber<TEvent>>().ToList();

        // if there are no subscribers, return early and cache that fact to avoid future lookups

        if (handlers.Count == 0) return Task.CompletedTask; // return early if no subscribers

        var handlerTasks = handlers
            .Select(handler => (Func<Task>)(() => handler.Handle(typedEvent, ct)));

        return handlersExecutor.Invoke(handlerTasks, ct);
    }
}