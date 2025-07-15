using MWR.MedWaytoR.Helpers;
using MWR.MedWaytoR.PubSub;

namespace MWR.MedWaytoR.PubSubImplementation;

internal static class EventExecutorsFactory
{
    // TODO: Get the required service provider instance from the scope

    /*private readonly Func<IEnumerable<Func<Task>>, CancellationToken, Task> _handlersExecutor =
        executionType switch
        {
            ExecutionType.Sequential => (handlers, ct) => handlers.RunAllInSequence(ct),
            ExecutionType.Parallel => (handlers, _) => handlers.RunAllInParallel(),
            _ => throw new InvalidOperationException(
                $"""
                 Unsupported execution type: {executionType}.
                 Expected values: {ExecutionType.Sequential}, {ExecutionType.Parallel}.
                 """)
        };*/

    public static IEventExecutor CreateFor(IEvent @event)
    {
        var tEvent = @event.GetType();

        return CreateFor(tEvent);
    }

    public static IEventExecutor CreateFor(Type eventType)
    {
        var executorType = typeof(EventExecutor<>).MakeGenericType(eventType);

        return (IEventExecutor)(Activator.CreateInstance(executorType) ??
                                MedWaytoRException.ThrowFailedToCreateInstanceOf(executorType));
    }
}