using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace MWR.MedWaytoR.DI;

internal static class DiExtensions
{
    public static IServiceCollection AddService<TService>(
        this IServiceCollection services,
        Func<IServiceProvider, TService> factory,
        ServiceLifetime lifetime)
        where TService : class, new()
    {
        return lifetime switch
        {
            ServiceLifetime.Singleton => services.AddSingleton(factory),
            ServiceLifetime.Scoped => services.AddScoped(factory),
            ServiceLifetime.Transient => services.AddTransient(factory),
            _ => Helpers.ThrowInvalidServiceLifeTimeException(nameof(lifetime), lifetime)
        };
    }

    public static IServiceCollection AddService<TService, TImplementation>(
        this IServiceCollection services,
        ServiceLifetime lifetime)
        where TImplementation : class, TService
        where TService : class
    {
        return lifetime switch
        {
            ServiceLifetime.Singleton => services.AddSingleton<TService, TImplementation>(),
            ServiceLifetime.Scoped => services.AddScoped<TService, TImplementation>(),
            ServiceLifetime.Transient => services.AddTransient<TService, TImplementation>(),
            _ => Helpers.ThrowInvalidServiceLifeTimeException(nameof(lifetime), lifetime)
        };
    }

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
            _ => Helpers.ThrowInvalidServiceLifeTimeException(nameof(lifetime), lifetime)
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
            _ => Helpers.ThrowInvalidServiceLifeTimeException(nameof(lifetime), lifetime)
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

    [DoesNotReturn]
    public static IServiceCollection ThrowInvalidServiceLifeTimeException(
        string paramName,
        ServiceLifetime paramValue)
    {
        throw new ArgumentOutOfRangeException(
            paramName,
            paramValue,
            """
            Invalid ServiceLifetime value provided.
            Valid values are: Singleton (0), Scoped (1), Transient (2).
            """);
    }
}