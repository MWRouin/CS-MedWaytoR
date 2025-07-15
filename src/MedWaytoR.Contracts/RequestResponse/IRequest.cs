namespace MWR.MedWaytoR.RequestResponse;

/// <summary>
/// Internal marker interface for the Request/Response system.<br />
/// Do not implement directly; use <see cref="IRequest{TResponse}"/> for requests requiring a response.
/// </summary>
public interface IRequest;

/// <summary>
/// Marks a request in the Request/Response system that expects a response of type <typeparamref name="TResponse"/>.
/// Implement this interface for requests that require a response.
/// </summary>
/// <typeparam name="TResponse">The type of the expected response.</typeparam>
public interface IRequest<out TResponse> : IRequest;