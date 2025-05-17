using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MWR.MedWaytoR.RequestResponse;
using MWR.MedWaytoR.RequestResponseImplementation;

namespace MWR.MedWaytoR.DI;

public static class RequestResponseDi
{
    public static IServiceCollection AddMedWayToR_RequestResponse(this IServiceCollection services,
        ServiceLifetime lifetime = (ServiceLifetime)2)
    {
        return AddMedWayToR_RequestResponse(services, [Assembly.GetCallingAssembly()], lifetime);
    }

    public static IServiceCollection AddMedWayToR_RequestResponse(this IServiceCollection services,
        IEnumerable<Assembly> assemblies,
        ServiceLifetime lifetime = (ServiceLifetime)2)
    {
        var scanner = new RequestResponseScanner(assemblies);

        var requestHandlersDictionary = scanner.FindAllHandlersGroupedByInterfaceType().ToDictionary();
        
        var requestResponsePipesDictionary = scanner.FindAllPipesGroupedByInterfaceType().ToDictionary();

        foreach (var handlerRecord in requestHandlersDictionary)
        {
            services.AddService(handlerRecord.Key, handlerRecord.Value, lifetime);
        }

        foreach (var pipesGroup in requestResponsePipesDictionary)
        {
            services.AddServices(pipesGroup.Key, pipesGroup.Value, lifetime);
        }

        services.AddSingleton(Helpers.CreateConcreteRequestDispatcher);

        return services;
    }
}

file static class Helpers
{
    public static IRequestDispatcher CreateConcreteRequestDispatcher(IServiceProvider provider)
    {
        return new ConcreteRequestDispatcher(new RequestExecutorsFactory(provider));
    }
}