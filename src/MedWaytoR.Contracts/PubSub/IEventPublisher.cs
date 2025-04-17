namespace MedWaytoR.PubSub;

public interface IEventPublisher
{
    Task Publish<TEvent>(TEvent @event, CancellationToken ct = default) 
        where TEvent : IEvent;

    Task Publish<TEvent>(IEnumerable<TEvent> events, CancellationToken ct = default) 
        where TEvent : IEvent;
}