namespace MWR.MedWaytoR.Config;

public class MedWaytoRConfig
{
    public PubSubConfig PubSubConfig { get; init; } = PubSubConfig.Default;

    public RequestResponseConfig RequestResponseConfig { get; init; } = RequestResponseConfig.Default;
}