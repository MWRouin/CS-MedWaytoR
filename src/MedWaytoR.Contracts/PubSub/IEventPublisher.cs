namespace MWR.MedWaytoR.PubSub;

/// <summary>
/// Publish events
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Executes the event-subscribers that are subscribed to the type of the event
    /// </summary>
    /// <param name="event">Event object</param>
    /// <param name="ct">Optional cancellation token</param>
    /// <returns></returns>
    Task PublishAsync(IEvent @event, CancellationToken ct = default);

    /// <summary>
    /// Foreach event execute the event-subscribers that are subscribed to the type of the event
    /// </summary>
    /// <param name="events">Enumerable of events</param>
    /// <param name="ct">Optional cancellation token</param>
    /// <returns></returns>
    Task PublishAsync(IEnumerable<IEvent> events, CancellationToken ct = default);
}