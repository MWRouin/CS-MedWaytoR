using MWR.MedWaytoR.PubSub;

namespace MWR.MedWaytoR.PubSubImplementation;

internal class EventExecutorsFactory(IServiceProvider serviceProvider)
{
    private ExecutionType _executionType = ExecutionType.Sequential;

    public IEventExecutor Create(IEvent @event)
    {
        var tEvent = @event.GetType();

        var tExecutor = typeof(EventExecutor<>).MakeGenericType(tEvent);

        var constructor = tExecutor.GetConstructors().Single();

        return (IEventExecutor)constructor.Invoke([_executionType, serviceProvider]);
    }

    public void SetExecutionType(ExecutionType executionType)
    {
        if (Enum.IsDefined(typeof(ExecutionType), executionType))
            throw new ArgumentOutOfRangeException(nameof(executionType));
        _executionType = executionType;
    }
}

internal enum ExecutionType
{
    Sequential,
    Parallel
}