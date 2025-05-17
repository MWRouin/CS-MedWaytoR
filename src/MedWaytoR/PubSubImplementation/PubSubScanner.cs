using System.Reflection;
using MWR.MedWaytoR.Helpers;
using MWR.MedWaytoR.PubSub;

namespace MWR.MedWaytoR.PubSubImplementation;

internal class PubSubScanner(IEnumerable<Assembly> assemblies) : IPubSubScanner
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

    public IDictionary<Type, IEnumerable<Type>> FindAllSubscribersGroubedByEventType()
    {
        return DefinedTypes
            .Where(Helpers.IsEventSubscriberPredicate())
            .GroupBy(Helpers.GetEventTypeForEventSubscriber)
            .ToDictionary(grouping => grouping.Key, IEnumerable<Type> (grouping) => grouping);
    }

    public IDictionary<Type, IEnumerable<Type>> FindAllSubscribersGroubedBySubscriberInerfaceType()
    {
        return DefinedTypes
            .Where(Helpers.IsEventSubscriberPredicate())
            .GroupBy(Helpers.GetInterfaceTypeForEventSubscriber)
            .ToDictionary(grouping => grouping.Key, IEnumerable<Type> (grouping) => grouping);
    }
}

file static class Helpers
{
    private static readonly Type EventSubscriberType = typeof(IEventSubscriber<>);

    public static Type GetEventTypeForEventSubscriber(Type eventSubscriber)
    {
        return eventSubscriber.GetInterfaces()
            .First(EventSubscriberType.GenericTypeEqualityPredicate())
            .GetGenericArguments()[0];
    }

    public static Type GetInterfaceTypeForEventSubscriber(Type eventSubscriber)
    {
        return eventSubscriber.GetInterfaces()
            .First(EventSubscriberType.GenericTypeEqualityPredicate());
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