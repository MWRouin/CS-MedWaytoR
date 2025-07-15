using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MWR.MedWaytoR.Helpers;

namespace MWR.MedWaytoR.Config;

public enum ExecutionType
{
    Sequential,
    Parallel
}

public class PubSubConfig
{
    public ExecutionType ExecutionType { get; init; } = ExecutionType.Sequential;

    public ServiceLifetime ServicesLifetime { get; init; } = ServiceLifetime.Scoped;

    public Assembly[] Assemblies { get; init; } = [Assembly.GetExecutingAssembly()];

    public static PubSubConfig Default => new();
}

internal class PubSubExecutorFunc
{
    public readonly Func<IEnumerable<Func<Task>>, CancellationToken, Task> ExecuteFunc;

    private PubSubExecutorFunc(ExecutionType executionType)
    {
        ExecuteFunc = executionType switch
        {
            ExecutionType.Sequential => LinqExtensions.RunAllInSequence,
            ExecutionType.Parallel => LinqExtensions.RunAllInParallel,
            _ => throw new ArgumentException(
                $"""
                 Unsupported execution type: {executionType}.
                 Expected values: {ExecutionType.Sequential}, {ExecutionType.Parallel}.
                 """, 
                nameof(executionType))
        };
    }

    public static PubSubExecutorFunc CreateFrom(PubSubConfig config)
    {
        return new PubSubExecutorFunc(config.ExecutionType);
    }
}