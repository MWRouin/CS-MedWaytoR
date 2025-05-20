using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MWR.MedWaytoR.PubSub;
using MWR.MedWaytoR.PubSubImplementation;

namespace MWR.MedWaytoR.DI;

public static class PubSubDi
{
    public static IServiceCollection AddMedWayToR_PubSub(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        return AddMedWayToR_PubSub(services, [Assembly.GetCallingAssembly()], lifetime);
    }

    public static IServiceCollection AddMedWayToR_PubSub(this IServiceCollection services,
        IEnumerable<Assembly> assemblies,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        var scanner = new PubSubScanner(assemblies);

        Console.WriteLine($"AddMedWayToR_PubSub ==> {lifetime.ToString()}");

        var eventSubscribersDictionary = scanner.FindAllSubscribersGroubedBySubscriberInerfaceType().ToDictionary();

        foreach (var subscribersGroup in eventSubscribersDictionary)
        {
            Console.WriteLine($"Subscribers: {subscribersGroup.Key} : {string.Join(", ", subscribersGroup.Value)}");
            services.AddServices(subscribersGroup.Key, subscribersGroup.Value, lifetime);
        }

        // TODO make singleton and fix appropriate service provider use
        services.AddScoped(Helpers.CreateConcreteEventPublisher);

        Console.WriteLine("AddMedWayToR_PubSub ==> End");

        return services;
    }
}

file static class Helpers
{
    public static IEventPublisher CreateConcreteEventPublisher(IServiceProvider provider)
    {
        return new ConcreteEventPublisher(new EventExecutorsFactory(provider));
    }
}