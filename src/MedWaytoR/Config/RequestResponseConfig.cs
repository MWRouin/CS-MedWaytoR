using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MWR.MedWaytoR.Config;

public class RequestResponseConfig
{
    public ServiceLifetime ServicesLifetime { get; init; } = ServiceLifetime.Scoped;

    public Assembly[] Assemblies { get; init; } = [Assembly.GetCallingAssembly()];

    public static RequestResponseConfig Default => new();
}