using MWR.MedWaytoR.Helpers;
using MWR.MedWaytoR.RequestResponse;

namespace MWR.MedWaytoR.RequestResponseImplementation;

internal static class RequestExecutorsFactory
{
    public static RequestExecutor<TRequest, TResponse> Create<TRequest, TResponse>()
        where TRequest : IRequest<TResponse>
    {
        return new RequestExecutor<TRequest, TResponse>();
    }

    // Using heavy reflection to create the executor type for the given request.
    public static IRequestExecutor<TResponse> CreateFor<TResponse>(IRequest<TResponse> request)
    {
        var requestType = request.GetType();

        return (IRequestExecutor<TResponse>)CreateFor(requestType);
    }

    // Using heavy reflection to create the executor type for the given request.
    public static IRequestExecutor CreateFor(IRequest request)
    {
        var requestType = request.GetType();

        return CreateFor(requestType);
    }

    // Using heavy reflection to create the executor type for the given request.
    public static IRequestExecutor CreateFor(Type requestType)
    {
        var responseType = requestType.GetInterfaces()
            .First(typeof(IRequest<>).GenericTypeDefinitionEqualityPredicate())
            .GetGenericArguments()[0];

        var executorType = typeof(RequestExecutor<,>).MakeGenericType(requestType, responseType);

        return (IRequestExecutor)(Activator.CreateInstance(executorType) ??
                                  MedWaytoRException.ThrowFailedToCreateInstanceOf(requestType));
    }
}