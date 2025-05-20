using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace MWR.MedWaytoR.DI;

internal static class DiExtensions
{
    public static IServiceCollection AddService(
        this IServiceCollection services,
        Type serviceType,
        Type implementationType,
        ServiceLifetime lifetime)
    {
        return lifetime switch
        {
            ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationType),
            ServiceLifetime.Scoped => services.AddScoped(serviceType, implementationType),
            ServiceLifetime.Transient => services.AddTransient(serviceType, implementationType),
            _ => throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime,
                Helpers.InvalidServiceLifeTimeMessage)
        };
    }

    public static IServiceCollection AddServices(
        this IServiceCollection services,
        Type serviceType,
        IEnumerable<Type> implementationTypes,
        ServiceLifetime lifetime)
    {
        return lifetime switch
        {
            ServiceLifetime.Singleton => services.AddSingletonServices(serviceType, implementationTypes),
            ServiceLifetime.Scoped => services.AddScopedServices(serviceType, implementationTypes),
            ServiceLifetime.Transient => services.AddTransientServices(serviceType, implementationTypes),
            _ => throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime,
                Helpers.InvalidServiceLifeTimeMessage)
        };
    }
}

file static class Helpers
{
    public static IServiceCollection AddSingletonServices(
        this IServiceCollection services,
        Type serviceType,
        IEnumerable<Type> implementationTypes)
    {
        foreach (var implementationType in implementationTypes)
            services.AddSingleton(serviceType, implementationType);
        return services;
    }

    public static IServiceCollection AddScopedServices(
        this IServiceCollection services,
        Type serviceType,
        IEnumerable<Type> implementationTypes)
    {
        foreach (var implementationType in implementationTypes)
            services.AddScoped(serviceType, implementationType);
        return services;
    }

    public static IServiceCollection AddTransientServices(
        this IServiceCollection services,
        Type serviceType,
        IEnumerable<Type> implementationTypes)
    {
        foreach (var implementationType in implementationTypes)
            services.AddTransient(serviceType, implementationType);
        return services;
    }

    public const string InvalidServiceLifeTimeMessage =
        """
        Invalid ServiceLifetime value!
        Possible values are: Singleton (0), Scoped (1), Transient(2)
        """;
}