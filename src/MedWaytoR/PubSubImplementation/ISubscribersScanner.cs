using System.Collections;
using MWR.MedWaytoR.PubSub;

namespace MWR.MedWaytoR.PubSubImplementation;

public interface ISubscribersScanner
{
    IEnumerable<Type> FindSubscribersForEventType<TEvent>() where TEvent : IEvent;

    IEnumerable<Type> FindSubscribersForEventType(Type evenType);

    IDictionary<Type, IEnumerable<Type>> FindAllEventSubscribers();
}