using System.Reflection;
using MWR.MedWaytoR.Helpers;
using MWR.MedWaytoR.RequestResponse;

namespace MWR.MedWaytoR.RequestResponseImplementation;

internal class RequestResponseScanner(IEnumerable<Assembly> assemblies) : IRequestHandlersScanner
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

    public IDictionary<Type, (Type HandlerType, IEnumerable<Type> PipeTypes)> FindAllRequestHandlers()
    {
        var handlerTypes = DefinedTypes
            .Where(Helpers.IsRequestHandlerPredicate())
            .GroupBy(Helpers.GetRequestTypeForRequestHandlerFunc())
            .ToDictionary(grouping => grouping.Key, grouping => grouping.First());

        var pipeTypes = DefinedTypes
            .Where(Helpers.IsReqResPipePredicate())
            .GroupBy(Helpers.GetRequestTypeForReqResPipeFunc())
            .Where(grouping => handlerTypes.ContainsKey(grouping.Key))
            .ToDictionary(grouping => grouping.Key, IEnumerable<Type> (grouping) => grouping);

        return handlerTypes.Select(keyValuePair =>
                KeyValuePair.Create(keyValuePair.Key, (keyValuePair.Value, pipeTypes[keyValuePair.Key])))
            .ToDictionary();
    }
}

file static class Helpers
{
    private static readonly Type RequestHandlerType = typeof(IRequestHandler<,>);

    private static readonly Type RequestResponsePipeType = typeof(IRequestResponsePipe<,>);

    public static Func<Type, Type> GetRequestTypeForRequestHandlerFunc()
    {
        return requestHandlerType => requestHandlerType.GetInterfaces()
            .First(RequestHandlerType.GenericTypeEqualityPredicate())
            .GetGenericArguments()[0];
    }

    public static Func<Type, Type> GetRequestTypeForReqResPipeFunc()
    {
        return requestHandlerType => requestHandlerType.GetInterfaces()
            .First(RequestResponsePipeType.GenericTypeEqualityPredicate())
            .GetGenericArguments()[0];
    }

    public static Func<Type, bool> IsRequestHandlerPredicate()
    {
        return clsType => clsType.IsPublicConcreteClass() &&
                          clsType.GetInterfaces()
                              .Any(RequestHandlerType.GenericTypeEqualityPredicate());
    }

    public static Func<Type, bool> IsRequestHandlerPredicate(Type requestType, Type responseType)
    {
        return clsType => clsType.IsPublicConcreteClass() &&
                          clsType.GetInterfaces()
                              .Any(RequestHandlerType.GenericTypeEqualityPredicate(requestType, responseType));
    }

    public static Func<Type, bool> IsReqResPipePredicate()
    {
        return clsType =>
            clsType.IsPublicConcreteClass() &&
            clsType.GetInterfaces()
                .Any(RequestResponsePipeType.GenericTypeEqualityPredicate());
    }

    public static Func<Type, bool> IsReqResPipePredicate(Type requestType, Type responseType)
    {
        return clsType =>
            clsType.IsPublicConcreteClass() &&
            clsType.GetInterfaces()
                .Any(RequestResponsePipeType.GenericTypeEqualityPredicate(requestType, responseType));
    }
}