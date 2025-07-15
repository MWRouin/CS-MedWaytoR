using MWR.MedWaytoR.PubSub;

namespace MWR.MedWaytoR.PubSubImplementation;

internal interface IEventExecutor
{
    Task Execute(
        IEvent @event, 
        IServiceProvider serviceProvider,
        Func<IEnumerable<Func<Task>>, CancellationToken, Task> handlersExecutor,
        CancellationToken ct = default);
}
