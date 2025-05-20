using System.Collections;
using MWR.MedWaytoR.PubSub;

namespace MWR.MedWaytoR.PubSubImplementation;

public interface IPubSubScanner
{
    IEnumerable<Type> FindSubscribersForEventType<TEvent>() where TEvent : IEvent;

    IEnumerable<Type> FindSubscribersForEventType(Type evenType);

    IDictionary<Type, IEnumerable<Type>> FindAllSubscribersGroubedByEventType();

    IDictionary<Type, IEnumerable<Type>> FindAllSubscribersGroubedBySubscriberInerfaceType();
}