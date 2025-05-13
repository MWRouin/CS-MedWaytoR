using MWR.MedWaytoR.Helpers;
using MWR.MedWaytoR.RequestResponse;

namespace MWR.MedWaytoR.RequestResponseImplementation;

internal class RequestExecutorsFactory(IServiceProvider serviceProvider)
{
    public IRequestExecutor Create(IRequest request)
    {
        var tRequest = request.GetType();

        var tResponse = tRequest.GetInterfaces()
            .First(typeof(IRequest<>).GenericTypeEqualityPredicate())
            .GetGenericArguments()[0];

        var tExecutor = typeof(RequestExecutor<,>).MakeGenericType(tRequest, tResponse);

        var constructor = tExecutor.GetConstructors().Single();

        return (IRequestExecutor)constructor.Invoke([serviceProvider]);
    }
}