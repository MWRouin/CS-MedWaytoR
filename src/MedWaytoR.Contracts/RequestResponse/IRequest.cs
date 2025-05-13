namespace MWR.MedWaytoR.RequestResponse;

public interface IRequest;

public interface IRequest<out TResponse> : IRequest;