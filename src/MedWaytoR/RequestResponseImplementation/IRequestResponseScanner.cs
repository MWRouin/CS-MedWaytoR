using MWR.MedWaytoR.RequestResponse;

namespace MWR.MedWaytoR.RequestResponseImplementation;

internal interface IRequestResponseScanner
{
    (Type? HandlerType, IEnumerable<Type> PipeTypes) FindHandlerForRequestType<TRequest, TResponse>()
        where TRequest : IRequest<TResponse>;

    (Type? HandlerType, IEnumerable<Type> PipeTypes) FindHandlerForRequestType(Type requestType);

    (Type? HandlerType, IEnumerable<Type> PipeTypes) FindHandlerForRequestType(Type requestType, Type responseType);

    IDictionary<Type, (Type HandlerType, IEnumerable<Type> PipeTypes)> FindAllHandlersGroupedByRequestType();

    IDictionary<Type, Type> FindAllHandlersGroupedByInterfaceType();

    IDictionary<Type, IEnumerable<Type>> FindAllPipesGroupedByInterfaceType();
}