using System.Reflection;
using MWR.MedWaytoR.Helpers;
using MWR.MedWaytoR.RequestResponse;

namespace MWR.MedWaytoR.RequestResponseImplementation;

internal class RequestResponseScanner(IEnumerable<Assembly> assemblies) : IRequestResponseScanner
{
    private IEnumerable<TypeInfo> DefinedTypes => assemblies.SelectMany(asm => asm.DefinedTypes);

    public (Type? HandlerType, IEnumerable<Type> PipeTypes) FindHandlerForRequestType<TRequest, TResponse>()
        where TRequest : IRequest<TResponse>
    {
        return FindHandlerForRequestType(typeof(TRequest), typeof(TResponse));
    }

    public (Type? HandlerType, IEnumerable<Type> PipeTypes) FindHandlerForRequestType(Type requestType)
    {
        var responseType = requestType.GenericTypeArguments.Single();
        return FindHandlerForRequestType(requestType, responseType);
    }

    public (Type? HandlerType, IEnumerable<Type> PipeTypes) FindHandlerForRequestType(
        Type requestType,
        Type responseType)
    {
        var handlerType = DefinedTypes
            .FirstOrDefault(Helpers.IsRequestHandlerPredicate(requestType, responseType));

        var pipeTypes = DefinedTypes
            .Where(Helpers.IsReqResPipePredicate(requestType, responseType)).ToArray();

        return (handlerType, pipeTypes);
    }

    public IDictionary<Type, (Type HandlerType, IEnumerable<Type> PipeTypes)> FindAllHandlersGroupedByRequestType()
    {
        var handlerTypes = DefinedTypes
            .Where(Helpers.IsRequestHandlerPredicate())
            .GroupBy(Helpers.GetRequestTypeForRequestHandler)
            .ToDictionary(grouping => grouping.Key, grouping => grouping.First());

        var pipeTypes = DefinedTypes
            .Where(Helpers.IsReqResPipePredicate())
            .GroupBy(Helpers.GetRequestTypeForReqResPipe)
            .Where(grouping => handlerTypes.ContainsKey(grouping.Key))
            .ToDictionary(grouping => grouping.Key, IEnumerable<Type> (grouping) => grouping);

        return handlerTypes.Select(keyValuePair =>
                KeyValuePair.Create(keyValuePair.Key, (keyValuePair.Value, pipeTypes[keyValuePair.Key])))
            .ToDictionary();
    }

    public IDictionary<Type, Type> FindAllHandlersGroupedByInterfaceType()
    {
        return DefinedTypes
            .Where(Helpers.IsRequestHandlerPredicate())
            .GroupBy(Helpers.GetInterfaceTypeForRequestHandler)
            .ToDictionary(grouping => grouping.Key, grouping => grouping.First());
    }

    public IDictionary<Type, IEnumerable<Type>> FindAllPipesGroupedByInterfaceType()
    {
        return  DefinedTypes
            .Where(Helpers.IsReqResPipePredicate())
            .GroupBy(Helpers.GetInterfaceTypeForReqResPipe)
            .ToDictionary(grouping => grouping.Key, IEnumerable<Type> (grouping) => grouping);
    }
}

file static class Helpers
{
    private static readonly Type GenericRequestHandlerType = typeof(IRequestHandler<,>);

    private static readonly Type GenericRequestResponsePipeType = typeof(IRequestResponsePipe<,>);

    public static Type GetRequestTypeForRequestHandler(Type requestHandlerType)
    {
        return requestHandlerType.GetInterfaces()
            .First(GenericRequestHandlerType.GenericTypeDefinitionEqualityPredicate())
            .GetGenericArguments()[0];
    }

    public static Type GetInterfaceTypeForRequestHandler(Type requestHandlerType)
    {
        return requestHandlerType.GetInterfaces()
            .First(GenericRequestHandlerType.GenericTypeDefinitionEqualityPredicate());
    }

    public static Type GetRequestTypeForReqResPipe(Type requestResponsePipeType)
    {
        return requestResponsePipeType.GetInterfaces()
            .First(GenericRequestResponsePipeType.GenericTypeDefinitionEqualityPredicate())
            .GetGenericArguments()[0];
    }

    public static Type GetInterfaceTypeForReqResPipe(Type requestResponsePipeType)
    {
        return requestResponsePipeType.GetInterfaces()
            .First(GenericRequestResponsePipeType.GenericTypeDefinitionEqualityPredicate());
    }

    public static Func<Type, bool> IsRequestHandlerPredicate()
    {
        return clsType => clsType.IsPublicConcreteClass() &&
                          clsType.GetInterfaces()
                              .Any(GenericRequestHandlerType.GenericTypeDefinitionEqualityPredicate());
    }

    public static Func<Type, bool> IsRequestHandlerPredicate(Type requestType, Type responseType)
    {
        return clsType => clsType.IsPublicConcreteClass() &&
                          clsType.GetInterfaces()
                              .Any(GenericRequestHandlerType.GenericTypeDefinitionEqualityPredicate(requestType, responseType));
    }

    public static Func<Type, bool> IsReqResPipePredicate()
    {
        return clsType =>
            clsType.IsPublicConcreteClass() &&
            clsType.GetInterfaces()
                .Any(GenericRequestResponsePipeType.GenericTypeDefinitionEqualityPredicate());
    }

    public static Func<Type, bool> IsReqResPipePredicate(Type requestType, Type responseType)
    {
        return clsType =>
            clsType.IsPublicConcreteClass() &&
            clsType.GetInterfaces()
                .Any(GenericRequestResponsePipeType.GenericTypeDefinitionEqualityPredicate(requestType, responseType));
    }
}