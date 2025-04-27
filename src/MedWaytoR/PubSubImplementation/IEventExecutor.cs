using MWR.MedWaytoR.PubSub;

namespace MWR.MedWaytoR.PubSubImplementation;

internal interface IEventExecutor
{
    Task Execute(IEvent @event, CancellationToken ct = default);
}