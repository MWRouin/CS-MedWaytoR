using System.Reflection;
using MWR.MedWaytoR.Helpers;
using MWR.MedWaytoR.PubSub;

namespace MWR.MedWaytoR.PubSubImplementation;

internal class SubscribersScanner(IEnumerable<Assembly> assemblies) : ISubscribersScanner
{
    private IEnumerable<TypeInfo> DefinedTypes => assemblies.SelectMany(asm => asm.DefinedTypes);

    public IEnumerable<Type> FindSubscribersForEventType<TEvent>() where TEvent : IEvent
    {
        return FindSubscribersForEventType(typeof(TEvent));
    }

    public IEnumerable<Type> FindSubscribersForEventType(Type evenType)
    {
        return DefinedTypes.Where(Helpers.IsEventSubscriberPredicate(evenType));
    }

    public IDictionary<Type, IEnumerable<Type>> FindAllEventSubscribers()
    {
        return DefinedTypes
            .Where(Helpers.IsEventSubscriberPredicate())
            .GroupBy(Helpers.GetEventTypeForEventSubscriberFunc())
            .ToDictionary(grouping => grouping.Key, IEnumerable<Type> (grouping) => grouping);
    }
}

file static class Helpers
{
    private static readonly Type EventSubscriberType = typeof(IEventSubscriber<>);

    public static Func<Type, Type> GetEventTypeForEventSubscriberFunc()
    {
        return eventSubscriber => eventSubscriber.GetInterfaces()
            .First(EventSubscriberType.GenericTypeEqualityPredicate())
            .GetGenericArguments()[0];
    }

    public static Func<Type, bool> IsEventSubscriberPredicate()
    {
        return classType => classType.IsPublicConcreteClass()
                            && classType.GetInterfaces()
                                .Any(EventSubscriberType.GenericTypeEqualityPredicate());
    }

    public static Func<Type, bool> IsEventSubscriberPredicate(Type evenType)
    {
        return classType => classType.IsPublicConcreteClass()
                            && classType.GetInterfaces()
                                .Any(EventSubscriberType.GenericTypeEqualityPredicate(evenType));
    }
}