namespace MWR.MedWaytoR.PubSub;

public interface IEventSubscriber<in TEvent> where TEvent : IEvent
{
    Task Handle(TEvent @event, CancellationToken ct);
}