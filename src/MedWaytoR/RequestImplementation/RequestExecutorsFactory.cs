using MWR.MedWaytoR.RequestResponse;

namespace MWR.MedWaytoR.RequestImplementation;

internal class RequestExecutorsFactory(IServiceProvider serviceProvider)
{
    public IRequestExecutor Create(IRequest @event)
    {
        var tEvent = @event.GetType();
        
        var tResponse = tEvent.GetGenericArguments().Single();

        var tExecutor = typeof(RequestExecutor<,>).MakeGenericType(tEvent, tResponse);

        var constructor = tExecutor.GetConstructors().Single();

        return (IRequestExecutor)constructor.Invoke([serviceProvider]);
    }
}