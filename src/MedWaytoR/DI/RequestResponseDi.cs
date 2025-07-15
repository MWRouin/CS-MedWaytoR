using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MWR.MedWaytoR.RequestResponse;
using MWR.MedWaytoR.RequestResponseImplementation;

namespace MWR.MedWaytoR.DI;

public static class RequestResponseDi
{
    public static IServiceCollection AddMedWayToR_RequestResponse(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        return AddMedWayToR_RequestResponse(services, [Assembly.GetCallingAssembly()], lifetime);
    }

    public static IServiceCollection AddMedWayToR_RequestResponse(this IServiceCollection services,
        IEnumerable<Assembly> assemblies,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        Console.WriteLine($"AddMedWayToR_RequestResponse ==> {lifetime.ToString()}");

        var assembliesArray = assemblies?.ToArray();

        if (assembliesArray == null || assembliesArray.Length == 0)
            throw new ArgumentException("Assemblies cannot be null or empty.", nameof(assemblies));

        var scanner = new RequestResponseScanner(assembliesArray);

        foreach (var assembly in assembliesArray) Console.WriteLine(assembly.FullName);

        var requestHandlersDictionary = scanner.FindAllHandlersGroupedByInterfaceType().ToDictionary();

        var requestResponsePipesDictionary = scanner.FindAllPipesGroupedByInterfaceType().ToDictionary();

        foreach (var handlerRecord in requestHandlersDictionary)
        {
            Console.WriteLine($"Handlers : {handlerRecord.Key} : {handlerRecord.Value}");

            services.AddService(handlerRecord.Key, handlerRecord.Value, lifetime);
        }

        foreach (var pipesGroup in requestResponsePipesDictionary)
        {
            Console.WriteLine($"Pipes : {pipesGroup.Key}");
            services.AddServices(pipesGroup.Key, pipesGroup.Value, lifetime);
        }

        services.AddScoped<IRequestDispatcher, ConcreteRequestDispatcher>();

        Console.WriteLine("AddMedWayToR_RequestResponse ==> End");

        return services;
    }
}