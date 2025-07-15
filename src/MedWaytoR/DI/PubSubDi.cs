using Microsoft.Extensions.DependencyInjection;
using MWR.MedWaytoR.Config;
using MWR.MedWaytoR.PubSub;
using MWR.MedWaytoR.PubSubImplementation;

namespace MWR.MedWaytoR.DI;

public static class PubSubDi
{
    public static IServiceCollection AddMedWayToR_PubSub(this IServiceCollection services,
        Func<PubSubConfig> configFactory)
    {
        return AddMedWayToR_PubSub(services, configFactory.Invoke());
    }

    public static IServiceCollection AddMedWayToR_PubSub(this IServiceCollection services,
        PubSubConfig? config = null)
    {
        config ??= PubSubConfig.Default;

        var scanner = new PubSubScanner(config.Assemblies);

        Console.WriteLine($"AddMedWayToR_PubSub ==> {config.ServicesLifetime.ToString()}");

        scanner.FindAllSubscribersGroubedBySubscriberInerfaceType()
            .Aggregate(services, Helpers.AggregateAddServicesFunc(config))
            .AddSingleton(PubSubExecutorFunc.CreateFrom(config))
            .AddService<IEventPublisher, ConcreteEventPublisher>(config.ServicesLifetime);

        return services;
    }
}

file static class Helpers
{
    public static Func<IServiceCollection, KeyValuePair<Type, IEnumerable<Type>>, IServiceCollection>
        AggregateAddServicesFunc(PubSubConfig config)
    {
        return (serviceCollection, group) =>
        {
            Console.WriteLine($"Subscribers: {group.Key} : {string.Join(", ", group.Value)}");

            serviceCollection.AddServices(group.Key, group.Value, config.ServicesLifetime);

            return serviceCollection;
        };
    }
}