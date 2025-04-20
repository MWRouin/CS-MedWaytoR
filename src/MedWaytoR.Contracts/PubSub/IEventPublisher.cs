namespace MedWaytoR.PubSub;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct = default) 
        where TEvent : IEvent;

    Task PublishAsync<TEvent>(IEnumerable<TEvent> events, CancellationToken ct = default) 
        where TEvent : IEvent;
}