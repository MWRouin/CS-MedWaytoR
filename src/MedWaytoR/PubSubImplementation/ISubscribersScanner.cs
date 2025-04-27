using System.Collections;
using MWR.MedWaytoR.PubSub;

namespace MWR.MedWaytoR.PubSubImplementation;

public interface ISubscribersScanner
{
    Task<IEnumerable<Type>> ScanForEventType<TEvent>() where TEvent : IEvent;

    Task<IEnumerable<Type>> ScanForEventType(Type evenType);

    Task<IDictionary<Type, IEnumerable<Type>>> ScanForAllEventTypes();
}