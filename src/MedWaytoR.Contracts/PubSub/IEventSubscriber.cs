namespace MWR.MedWaytoR.PubSub;

public interface IEventSubscriber<in TEvent> where TEvent : IEvent
{
    /// <summary>
    /// Defines the method to handle an event
    /// </summary>
    /// <param name="event">The event to handle by the subscriber</param>
    /// <param name="ct">The propagated cancellation token</param>
    /// <returns></returns>
    Task Handle(TEvent @event, CancellationToken ct);
}