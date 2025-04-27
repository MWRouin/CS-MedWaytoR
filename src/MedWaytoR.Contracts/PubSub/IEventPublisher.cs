namespace MWR.MedWaytoR.PubSub;

/// <summary>
/// Publish events
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Execute the appropriate event subscribers for an event
    /// </summary>
    /// <param name="event">Event object</param>
    /// <param name="ct">Optional cancellation token</param>
    /// <returns></returns>
    Task PublishAsync(IEvent @event, CancellationToken ct = default);

    /// <summary>
    /// Execute the appropriate event subscribers' handler for the events
    /// </summary>
    /// <param name="events">Enumerable of events</param>
    /// <param name="ct">Optional cancellation token</param>
    /// <returns></returns>
    Task PublishAsync(IEnumerable<IEvent> events, CancellationToken ct = default);
}